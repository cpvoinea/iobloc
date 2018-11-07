using System;
using System.Collections.Generic;

namespace iobloc
{
    class TableController
    {
        const int L = 28;
        private readonly TableModel _model;
        private readonly Random _random = new Random();
        private int _currentPlayer;
        private bool White => _currentPlayer == 0;
        private int _selection;
        private TableLine Selected => _model[_selection];
        private readonly List<int> _picked = new List<int>();
        private readonly List<int> _dice = new List<int>();
        private readonly List<int> _diceValues = new List<int>();
        private readonly List<TableMove> _allowedMoves = new List<TableMove>();

        public TableState State { get; private set; }

        public TableController(TableModel model)
        {
            _model = model;
        }

        public void Initialize()
        {
            for (int i = 0; i < L; i++)
                _model[i].Clear();
            _model[0].Set(2, false);
            _model[5].Set(5, true);
            _model[7].Set(3, true);
            _model[11].Set(5, false);
            _model[12].Set(5, true);
            _model[16].Set(3, false);
            _model[18].Set(5, false);
            _model[23].Set(2, true);

            _currentPlayer = 0;
            _selection = 0;
            _picked.Clear();
            BeginTurn();
        }

        public void MoveLeft()
        {
            if (_selection >= L - 1)
                return;
            Selected.Select(false);
            _selection++;
            Selected.Select(true);
        }

        public void MoveRight()
        {
            if (_selection <= 0)
                return;
            Selected.Select(false);
            _selection--;
            Selected.Select(true);
        }

        public void Pick()
        {
            if (Selected.Count == 0 || Selected.IsPlayerWhite != White)
                return;
            Selected.Pick();
            _picked.Add(_selection);
        }

        public void Put()
        {
            if (_picked.Count == 0 || Selected.Count > 1 && Selected.IsPlayerWhite != White)
                return;
            if (Selected.IsPlayerWhite != White)
            {
                Selected.Take();
                _model[24 + (1 - _currentPlayer)].Put(!White);
            }

            int from = _picked[0];
            _model[from].Unpick();
            Selected.Put(White);
            _picked.RemoveAt(0);
        }

        private void BeginTurn()
        {
            ThrowDice();

            if (_model[26 + _currentPlayer].Count == 15)
                State = TableState.Ended;
            else
                _currentPlayer = 1 - _currentPlayer;
        }

        private void ThrowDice()
        {
            int d1 = _random.Next(6) + 1;
            int d2 = _random.Next(6) + 1;
            _dice.Clear();
            _dice.Add(d1);
            if (d1 == d2)
                for (int i = 0; i < 3; i++)
                    _dice.Add(d1);
            else
                _dice.Add(d2);

            SetDiceValues();
            SetAllowedMoves();
            ShowDice();
        }

        private void HighlightAllowedFrom()
        {
            int max = 0;
            foreach (var m in _allowedMoves)
            {
                _model[m.From].Select(true, true);
                int d = Math.Abs(m.From - _selection);
                if (d > max)
                {
                    max = d;
                    _selection = m.From;
                }
            }
            Selected.Select(true);
        }

        private void SetDiceValues()
        {
            _diceValues.Clear();
            if (_dice.Count == 0)
                return;

            int d1 = _dice[0];
            _diceValues.Add(d1);
            if (_dice.Count > 1)
            {
                int d2 = _dice[1];
                if (d1 != d2)
                {
                    _diceValues.Add(d2);
                    _diceValues.Add(d1 + d2);
                }
                else for (int i = 1; i < _dice.Count; i++)
                        _diceValues.Add(d1 * (i + 1));
            }
        }

        private void SetAllowedMoves()
        {
            _allowedMoves.Clear();
            if (_dice.Count == 0 || _diceValues.Count == 0)
                return;

            if (_model[24 + _currentPlayer].Count > 0)
            {
                int from = 24 + _currentPlayer;
                for (int i = 0; i < 6; i++)
                    if (_diceValues.Contains(i + 1))
                    {
                        int to = White ? 23 - i : i;
                        if (_model[to].Count <= 1 || _model[to].IsPlayerWhite == White)
                            _allowedMoves.Add(new TableMove(from, to));
                    }
            }
            else
            {
                for (int from = 0; from < 24; from++)
                    if (_model[from].Count > 0 && _model[from].IsPlayerWhite == White)
                        foreach (int v in _diceValues)
                        {
                            int to = White ? from - v : from + v;
                            if (to >= 0 && to < 24 && (_model[to].Count <= 1 || _model[to].IsPlayerWhite == White))
                                _allowedMoves.Add(new TableMove(from, to));
                        }
                bool canTakeOut = true;
                for (int i = 0; i < 18 && canTakeOut; i++)
                {
                    int line = White ? i + 6 : i;
                    if (_model[line].Count > 0 && _model[line].IsPlayerWhite == White)
                        canTakeOut = false;
                }
                if (canTakeOut)
                {
                    int to = 26 + _currentPlayer;
                    for (int i = 0; i < 6; i++)
                    {
                        int from = White ? i : 23 - i;
                        if (_model[from].Count > 0 && _model[from].IsPlayerWhite == White)
                        {
                            bool hasValueToTakeOut = false;
                            foreach (int v in _diceValues)
                                if (v >= i + 1)
                                    hasValueToTakeOut = true;
                            if (hasValueToTakeOut)
                                _allowedMoves.Add(new TableMove(from, to));
                        }
                    }
                }
            }
        }

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }
    }
}
