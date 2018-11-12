using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly SortedDictionary<LineType, List<LineType>> _allowedMoves = new SortedDictionary<LineType, List<LineType>>();
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
            ShowAllowed();
        }

        public void Move(bool left)
        {
            ShowAllowed();
        }

        public void Action()
        {
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
            ShowDice();
        }

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }

        private void ShowAllowed()
        {
            _allowedMoves.Clear();
            _model.ClearSelection();
            if (_dice.Count == 0)
            {
                EndTurn();
                return;
            }

            SetAllowed();
            if(_allowedMoves.Count == 0)
            {
                EndTurn();
                return;
            }
            foreach(LineType line in _allowedMoves.Keys)
                _model[_player, line].Select(true, true);

            _cursor = _allowedMoves.Keys.Last();
            Selected.Select(true);
        }

        private void SetAllowed()
        {
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
        }
    }
}