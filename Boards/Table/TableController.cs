using System;
using System.Collections.Generic;

namespace iobloc
{
    class TableController
    {
        private readonly TableModel _model;
        private readonly ITableAI _player1;
        private readonly ITableAI _player2;
        private readonly Random _random = new Random();
        private readonly List<int> _dice = new List<int>();
        private readonly List<LineType> _allowed = new List<LineType>();
        private readonly Queue<Action> _actionQueue = new Queue<Action>();
        private PlayerSide _side;
        private LineType? _cursor;
        private LineType? _pickedFrom;
        private ITableAI CurrentPlayer => _side == PlayerSide.White ? _player1 : _player2;
        public bool CurrentPlayerIsAI => CurrentPlayer != null;
        public GameState State { get; private set; }

        public TableController(TableModel model, ITableAI player1, ITableAI player2)
        {
            _model = model;
            _player1 = player1;
            _player2 = player2;
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

            if (CurrentPlayerIsAI)
            {
                _actionQueue.Clear();
                var moves = CurrentPlayer.GetMoves(_model.GetLines(_side), _dice.ToArray());
                foreach (var m in moves)
                {
                    LineType from = _model.GetLineType(_side, m[0]);
                    LineType to = _model.GetLineType(_side, m[1]);
                    _actionQueue.Enqueue(new Action(ActionType.Pick, from));
                    _actionQueue.Enqueue(new Action(ActionType.Put, to));
                }

                PlayerAction();
            }
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

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }

        private void SetAllowedFrom()
        {
            if (_model[_side, LineType.Taken].Count > 0)
            {
                LineType from = LineType.Taken;
                for (int entry = 1; entry <= 6; entry++)
                {
                    LineType to = (LineType)(24 - entry);
                    if (_dice.Contains(entry) && _model[_side, to].CanPut(_side))
                        Add(from);
                }
            }
            else
            {
                for (int idxFrom = 0; idxFrom < 24; idxFrom++)
                {
                    LineType from = (LineType)idxFrom;
                    if (_model[_side, from].HasAny(_side))
                        for (int idxTo = idxFrom - 1; idxTo >= 0 && idxTo >= idxFrom - 6; idxTo--)
                        {
                            int d = idxFrom - idxTo;
                            LineType to = (LineType)idxTo;
                            if (_dice.Contains(d) && _model[_side, to].CanPut(_side))
                                Add(from);
                        }
                }

                bool canTakeOut = true;
                for (int i = 0; i < 18 && canTakeOut; i++)
                {
                    LineType line = (LineType)(i + 6);
                    if (_model[_side, line].HasAny(_side))
                        canTakeOut = false;
                }
                if (canTakeOut)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        LineType from = (LineType)i;
                        if (_model[_side, from].HasAny(_side))
                        {
                            if (_dice.Contains(i + 1))
                                Add(from);
                            else
                            {
                                bool clearLeft = true;
                                for (int j = i + 1; j < 6 && clearLeft; j++)
                                    if (_model[_side, (LineType)j].HasAny(_side))
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

        private void SetAllowedTo(LineType from)
        {
            Add(from);
            if (from == LineType.Taken)
                for (int entry = 1; entry <= 6; entry++)
                {
                    LineType to = (LineType)(24 - entry);
                    if (_dice.Contains(entry) && _model[_side, to].CanPut(_side))
                        Add(to);
                }
            else
            {
                int idxFrom = (int)from;
                for (int idxTo = idxFrom - 1; idxTo >= 0 && idxTo >= idxFrom - 6; idxTo--)
                {
                    int dif = idxFrom - idxTo;
                    LineType to = (LineType)idxTo;
                    if (_dice.Contains(dif) && _model[_side, to].CanPut(_side))
                        Add(to);
                }

                if (_allowed.Contains(LineType.Out))
                    return;
                bool canTakeOut = true;
                for (int i = 0; i < 18 && canTakeOut; i++)
                {
                    LineType line = (LineType)(i + 6);
                    if (_model[_side, line].HasAny(_side))
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
                        if (_model[_side, (LineType)j].HasAny(_side))
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
                if (_model[_side, LineType.Out].Count == 15)
                    State = GameState.Ended;
                else
                    EndTurn();
                return;
            }

            _model.ClearSelection();
            foreach (LineType line in _allowed)
                _model[_side, line].Select(true, true);
        }

        private void Add(LineType line)
        {
            if (!_allowed.Contains(line))
                _allowed.Add(line);
        }

        private void EndTurn()
        {
            _side = (PlayerSide)(1 - _side);
            ThrowDice();
        }

        private void Pick(LineType from)
        {
            _model[_side, from].Pick();
            _pickedFrom = from;
        }

        private void Unpick(LineType from)
        {
            var line = _model[_side, from];
            line.Unpick();
            line.Put(_side);
            _pickedFrom = null;
        }

        private void Put(LineType to)
        {
            // unpick
            _model[_side, _pickedFrom.Value].Unpick();

            var line = _model[_side, to];
            // capture
            if (line.Player != _side && line.Count == 1)
            {
                line.Take();
                var opponent = (PlayerSide)(1 - _side);
                _model[opponent, LineType.Taken].Put(opponent);
            }
            // put
            line.Put(_side);
            UpdateDice(_pickedFrom.Value, to);
            _pickedFrom = null;
        }

        public void CursorMove(bool left)
        {
            if (_allowed.Count == 0)
                return;
            if (_cursor.HasValue)
            {
                _model[_side, _cursor.Value].Select(false);
                int i = _allowed.IndexOf(_cursor.Value) + (left ? -1 : 1);
                if (i < 0)
                    i = _allowed.Count - 1;
                if (i >= _allowed.Count)
                    i = 0;
                _cursor = _allowed[i];
            }
            else
                _cursor = _allowed[_allowed.Count - 1];

            _model[_side, _cursor.Value].Select(true);
        }

        public void CursorAction()
        {
            if (_cursor == null)
                return;

            if (_pickedFrom.HasValue && _cursor != _pickedFrom) // put
                Put(_cursor.Value);
            else // take/put back
            {
                if (!_pickedFrom.HasValue) // take
                    Pick(_cursor.Value);
                else if (_cursor == _pickedFrom) // put back
                    Unpick(_cursor.Value);
            }
            ShowAllowed();
        }

        public void PlayerAction()
        {
            if (_actionQueue.Count == 0)
                return;

            var action = _actionQueue.Dequeue();
            switch (action.Type)
            {
                case ActionType.Pick:
                    Pick(action.Line);
                    break;
                case ActionType.Put:
                    Put(action.Line);
                    break;
            }

            ShowAllowed();
        }
    }
}
