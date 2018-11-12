using System;
using System.Collections.Generic;

namespace iobloc
{
    class TableController
    {
        private readonly TableModel _model;
        private readonly Random _random = new Random();
        private PlayerSide _player;
        private LineType? _cursor;
        private LineType? _pickedFrom;
        private int _pickedCount;
        private readonly List<int> _dice = new List<int>();
        private readonly Dictionary<LineType, List<LineType>> _allowedMoves = new Dictionary<LineType, List<LineType>>();
        public State State { get; private set; }
        private TableLine Selected => _cursor.HasValue ? _model[_player, _cursor.Value] : null;
        private PlayerSide OtherPlayer => (PlayerSide)(1 - _player);

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
            _pickedCount = 0;
            ThrowDice();
        }

        public void Move(bool left)
        {
            if (_allowedMoves.Count == 0)
                return;
            List<int> allowed = new List<int>();
            if (_pickedFrom.HasValue)
            {
                allowed.Add((int)_pickedFrom.Value);
                if (_allowedMoves.ContainsKey(_pickedFrom.Value))
                    foreach (LineType to in _allowedMoves[_pickedFrom.Value])
                        allowed.Add((int)to);
            }
            else
                foreach (LineType from in _allowedMoves.Keys)
                    allowed.Add((int)from);
            allowed.Sort();
            if (_cursor.HasValue)
            {
                Selected.Select(false);
                int i = allowed.IndexOf((int)_cursor.Value);
                i += left ? -1 : 1;
                if (i < 0)
                    i = allowed.Count - 1;
                if (i >= allowed.Count)
                    i = 0;
                _cursor = (LineType)allowed[i];
            }
            else
                _cursor = left ? (LineType)allowed[allowed.Count - 1] : (LineType)allowed[0];
            Selected.Select(true);
        }

        public void Action(bool pick)
        {
            if (_cursor == null)
                return;

            if (_pickedCount > 0 && _cursor != _pickedFrom)
            {
                _model[_player, _pickedFrom.Value].Unpick();
                Selected.Put(_player);
                int d = _pickedFrom.Value - _cursor.Value;
                // TODO matrix 26x26 of allowed moves with cost = dice (?)
                if (_pickedFrom == LineType.Taken)
                    d = 24 - (int)_cursor.Value;
                else if (_cursor.Value == LineType.Out)
                {
                    d = (int)_pickedFrom.Value + 1;
                    if (!_dice.Contains(d))
                        foreach (int dd in _dice)
                            if (dd > d)
                            {
                                d = dd;
                                break;
                            }
                }

                _pickedCount--;
                if (_pickedCount == 0)
                    _pickedFrom = null;
                _dice.Remove(d);
                ShowDice();
                ShowAllowedFrom();
            }
            else
            {
                if (pick)
                {
                    Selected.Pick();
                    _pickedCount++;
                    _pickedFrom = _cursor;
                    ShowAllowedTo();
                }
                else
                {
                    Selected.Unpick();
                    Selected.Put(_player);
                    _pickedCount--;
                    if (_pickedCount == 0)
                    {
                        ShowAllowedFrom();
                        _pickedFrom = null;
                    }
                }
            }
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
            ShowDice();
            ShowAllowedFrom();
        }

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }

        private void ShowAllowedFrom()
        {
            _model.ClearSelection();
            _cursor = null;
            if (_dice.Count == 0)
            {
                EndTurn();
                return;
            }

            SetAllowed();

            if (_allowedMoves.Count == 0)
            {
                EndTurn();
                return;
            }
            // TODO treat _allowedMoves.Count <= _dice.Count, check for max number of possible moves
            foreach (LineType line in _allowedMoves.Keys)
                _model[_player, line].Select(true, true);
            Move(true);
        }

        private void ShowAllowedTo()
        {
            if (!_pickedFrom.HasValue)
                return;
            _model.ClearSelection();
            _model[_player, _pickedFrom.Value].Select(true, true);
            foreach (var to in _allowedMoves[_pickedFrom.Value])
                _model[_player, to].Select(true, true);

            if (_pickedFrom.HasValue)
            {
                _cursor = _pickedFrom;
                Selected.Select(true);
            }
            else
            {
                _cursor = null;
                Move(true);
            }
        }

        private void SetAllowed()
        {
            _allowedMoves.Clear();
            if (_model[_player, LineType.Taken].Count > 0)
            {
                LineType from = LineType.Taken;
                for (int entry = 1; entry <= 6; entry++)
                {
                    LineType to = (LineType)(24 - entry);
                    if (_dice.Contains(entry) && _model[_player, to].CanPut(_player))
                        AddMove(from, to);
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
                                AddMove(from, to);
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
                    LineType to = LineType.Out;
                    for (int i = 1; i <= 6; i++)
                    {
                        LineType from = (LineType)(i - 1);
                        if (_model[_player, from].HasAny(_player))
                        {
                            if (_dice.Contains(i))
                                AddMove(from, to);
                            else
                            {
                                bool clearLeft = true;
                                for (int j = i + 1; j <= 6 && clearLeft; j++)
                                    if (_model[_player, (LineType)j].HasAny(_player))
                                        clearLeft = false;
                                if (clearLeft)
                                    for (int j = i + 1; j <= 6; j++)
                                        if (_dice.Contains(j))
                                            AddMove(from, to);
                            }
                        }
                    }
                }
            }
        }

        private void AddMove(LineType from, LineType to)
        {
            if (_allowedMoves.ContainsKey(from))
            {
                if (!_allowedMoves[from].Contains(to))
                    _allowedMoves[from].Add(to);
            }
            else
                _allowedMoves.Add(from, new List<LineType> { to });
        }

        private void EndTurn()
        {
            _player = OtherPlayer;
            ThrowDice();
        }
    }
}
