using System;
using System.Collections.Generic;

namespace iobloc
{
    class TableController
    {
        private readonly TableModel _model;
        private readonly Random _random = new Random();
        private readonly List<int> _dice = new List<int>();
        private readonly List<LineType> _allowed = new List<LineType>();
        private PlayerSide _player;
        private LineType? _cursor;
        private LineType? _pickedFrom;
        private TableLine Selected => _cursor.HasValue ? _model[_player, _cursor.Value] : null;
        private PlayerSide OtherPlayer => (PlayerSide)(1 - _player);
        public bool CurrentPlayerIsAI => true;
        public GameState State { get; private set; }

        public TableController(TableModel model)
        {
            _model = model;
        }

        public void Initialize()
        {
            _model.Clear();
            _model.ClearSelection();
            _model[PlayerSide.White, LineType.Line1].Initialize(2, PlayerSide.Black);
            _model[PlayerSide.White, LineType.Line6].Initialize(5, PlayerSide.White);
            _model[PlayerSide.White, LineType.Line8].Initialize(3, PlayerSide.White);
            _model[PlayerSide.White, LineType.Line12].Initialize(5, PlayerSide.Black);
            _model[PlayerSide.White, LineType.Line13].Initialize(5, PlayerSide.White);
            _model[PlayerSide.White, LineType.Line17].Initialize(3, PlayerSide.Black);
            _model[PlayerSide.White, LineType.Line19].Initialize(5, PlayerSide.Black);
            _model[PlayerSide.White, LineType.Line24].Initialize(2, PlayerSide.White);

            _cursor = null;
            _pickedFrom = null;
            State = GameState.Running;
            ThrowDice();
        }

        public void Move(bool left)
        {
            if (_allowed.Count == 0)
                return;
            if (_cursor.HasValue)
            {
                Selected.Select(false);
                int i = _allowed.IndexOf(_cursor.Value) + (left ? -1 : 1);
                if (i < 0)
                    i = _allowed.Count - 1;
                if (i >= _allowed.Count)
                    i = 0;
                _cursor = _allowed[i];
            }
            else
                _cursor = _allowed[_allowed.Count - 1];
            Selected.Select(true);
        }

        public void Action()
        {
            if (_cursor == null)
                return;

            if (_pickedFrom.HasValue && _cursor != _pickedFrom) // put
            {
                // unpick
                _model[_player, _pickedFrom.Value].Unpick();
                // capture
                if (Selected.Player != _player && Selected.Count == 1)
                {
                    Selected.Take();
                    _model[OtherPlayer, LineType.Taken].Put(OtherPlayer);
                }
                // put
                Selected.Put(_player);
                UpdateDice(_pickedFrom.Value, _cursor.Value);
                _pickedFrom = null;
            }
            else // take/put back
            {
                if (!_pickedFrom.HasValue) // take
                {
                    Selected.Pick();
                    _pickedFrom = _cursor;
                }
                else if (_cursor == _pickedFrom) // put back
                {
                    Selected.Unpick();
                    Selected.Put(_player);
                    _pickedFrom = null;
                }
            }
            ShowAllowed();
        }

        private void ThrowDice()
        {
            int d1 = _random.Next(6) + 1;
            int d2 = _random.Next(6) + 1;
            _dice.Clear();
            _dice.Add(d1);
            _dice.Add(d2);
            if (d1 == d2)
                _dice.AddRange(_dice);
            _dice.Sort();
            ShowDice();
            ShowAllowed();
        }

        private void UpdateDice(LineType from, LineType to)
        {
            int d = from - to;
            if (from == LineType.Taken)
                d = 24 - (int)to;
            else if (to == LineType.Out)
                foreach (int dd in _dice)
                    if (dd >= d)
                    {
                        d = dd;
                        break;
                    }
            _dice.Remove(d);
            ShowDice();
        }

        private void ShowAllowed()
        {
            if (_dice.Count == 0)
            {
                EndTurn();
                return;
            }

            _allowed.Clear();
            if (_pickedFrom.HasValue)
                SetAllowedTo(_pickedFrom.Value);
            else
                SetAllowedFrom();
            _allowed.Sort();
            // TODO treat _allowedMoves.Count <= _dice.Count, check for max number of possible moves
            if (_allowed.Count == 0)
            {
                if (_model[_player, LineType.Out].Count == 15)
                    State = GameState.Ended;
                else
                    EndTurn();
                return;
            }

            _model.ClearSelection();
            foreach (LineType line in _allowed)
                _model[_player, line].Select(true, true);

            if (!_pickedFrom.HasValue && _cursor.HasValue && _allowed.Contains(_cursor.Value))
                Selected.Select(true);
            else
            {
                _cursor = null;
                Move(_pickedFrom.HasValue);
            }
        }

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }

        private void SetAllowedTo(LineType from)
        {
            Add(from);
            if (from == LineType.Taken)
                for (int entry = 1; entry <= 6; entry++)
                {
                    LineType to = (LineType)(24 - entry);
                    if (_dice.Contains(entry) && _model[_player, to].CanPut(_player))
                        Add(to);
                }
            else
            {
                int idxFrom = (int)from;
                for (int idxTo = idxFrom - 1; idxTo >= 0 && idxTo >= idxFrom - 6; idxTo--)
                {
                    int dif = idxFrom - idxTo;
                    LineType to = (LineType)idxTo;
                    if (_dice.Contains(dif) && _model[_player, to].CanPut(_player))
                        Add(to);
                }

                if (_allowed.Contains(LineType.Out))
                    return;
                bool canTakeOut = true;
                for (int i = 0; i < 18 && canTakeOut; i++)
                {
                    LineType line = (LineType)(i + 6);
                    if (_model[_player, line].HasAny(_player))
                        canTakeOut = false;
                }
                if (!canTakeOut)
                    return;

                int d = (int)from + 1;
                if (_dice.Contains(d))
                    Add(LineType.Out);
                else
                {
                    bool clearLeft = true;
                    for (int j = d; j < 6 && clearLeft; j++)
                        if (_model[_player, (LineType)j].HasAny(_player))
                            clearLeft = false;
                    if (clearLeft)
                        for (int j = d; j <= 6; j++)
                            if (_dice.Contains(j))
                            {
                                Add(LineType.Out);
                                return;
                            }
                }
            }
        }

        private void SetAllowedFrom()
        {
            if (_model[_player, LineType.Taken].Count > 0)
            {
                LineType from = LineType.Taken;
                for (int entry = 1; entry <= 6; entry++)
                {
                    LineType to = (LineType)(24 - entry);
                    if (_dice.Contains(entry) && _model[_player, to].CanPut(_player))
                        Add(from);
                }
            }
            else
            {
                for (int idxFrom = 0; idxFrom < 24; idxFrom++)
                {
                    LineType from = (LineType)idxFrom;
                    if (_model[_player, from].HasAny(_player))
                        for (int idxTo = idxFrom - 1; idxTo >= 0 && idxTo >= idxFrom - 6; idxTo--)
                        {
                            int d = idxFrom - idxTo;
                            LineType to = (LineType)idxTo;
                            if (_dice.Contains(d) && _model[_player, to].CanPut(_player))
                                Add(from);
                        }
                }

                bool canTakeOut = true;
                for (int i = 0; i < 18 && canTakeOut; i++)
                {
                    LineType line = (LineType)(i + 6);
                    if (_model[_player, line].HasAny(_player))
                        canTakeOut = false;
                }
                if (canTakeOut)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        LineType from = (LineType)i;
                        if (_model[_player, from].HasAny(_player))
                        {
                            if (_dice.Contains(i + 1))
                                Add(from);
                            else
                            {
                                bool clearLeft = true;
                                for (int j = i + 1; j < 6 && clearLeft; j++)
                                    if (_model[_player, (LineType)j].HasAny(_player))
                                        clearLeft = false;
                                if (clearLeft)
                                    for (int j = i + 2; j <= 6; j++)
                                        if (_dice.Contains(j))
                                            Add(from);
                            }
                        }
                    }
                }
            }
        }

        private void Add(LineType line)
        {
            if (!_allowed.Contains(line))
                _allowed.Add(line);
        }

        private void EndTurn()
        {
            _player = OtherPlayer;
            ThrowDice();
        }
    }
}
