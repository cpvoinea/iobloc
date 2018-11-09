using System;
using System.Collections.Generic;

namespace iobloc
{
    class TableController
    {
        const int L = 28;
        const int TW = 24;
        const int TB = 25;
        const int OW = 26;
        const int OB = 27;
        private readonly TableModel _model;
        private readonly Random _random = new Random();
        private int _currentPlayer;
        private int _cursor;
        private int _pickedFrom;
        private int _pickedCount;
        private bool _diceDouble;
        private readonly List<int> _dice = new List<int>();
        private readonly List<int> _diceValues = new List<int>();
        private readonly List<int> _allowed = new List<int>();
        private bool White => _currentPlayer == 0;
        private TableLine Selected => _model[_cursor];
        public TableState State { get; private set; }

        public TableController(TableModel model)
        {
            _model = model;
        }

        public void Initialize()
        {
            _model.Clear();
            _model.ClearSelection();
            _model[0].Initialize(2, false);
            _model[5].Initialize(5, true);
            _model[7].Initialize(3, true);
            _model[11].Initialize(5, false);
            _model[12].Initialize(5, true);
            _model[16].Initialize(3, false);
            _model[18].Initialize(5, false);
            _model[23].Initialize(2, true);

            _cursor = -1;
            _pickedFrom = -1;
            _pickedCount = 0;
            BeginTurn();
        }

        public void Move(bool left)
        {
            if (_cursor >= 0)
                Selected.Select(false);
            if (_allowed.Count == 0)
                _cursor = -1;
            else
            {
                int i = _cursor >= 0 ? _allowed.IndexOf(_cursor) : -1;
                if (left)
                {
                    i--;
                    if (i < 0)
                        i = _allowed.Count - 1;
                }
                else
                {
                    i++;
                    if (i >= _allowed.Count)
                        i = 0;
                }
                _cursor = _allowed[i];
                Selected.Select(true);
            }
        }

        public void Action()
        {
            if (_pickedCount > 0 && _pickedFrom != _cursor)
            {
                _model[_pickedFrom].Unpick();
                Selected.Put(White);
                _pickedCount--;
                if (_pickedCount == 0)
                    _pickedFrom = -1;
            }
            else
            {
                Selected.Pick();
                _pickedCount++;
                _pickedFrom = _cursor;
            }
            ShowAllowed();
        }

        private void BeginTurn()
        {
            if (_cursor >= 0)
                Selected.Select(false);
            ThrowDice();
            if (_cursor >= 0)
                Selected.Select(true);
            else
            {
                _currentPlayer = 1 - _currentPlayer;
                // BeginTurn();
            }
        }

        private void ThrowDice()
        {
            int d1 = _random.Next(6) + 1;
            int d2 = _random.Next(6) + 1;
            _diceDouble = d1 == d2;
            _dice.Clear();
            _dice.Add(d1);
            _dice.Add(d2);
            if (_diceDouble)
            {
                _dice.Add(d1);
                _dice.Add(d1);
            }

            SetDiceValues();
            ShowDice();
            ShowAllowed();
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
                if (_diceDouble)
                    for (int i = 1; i < _dice.Count; i++)
                        _diceValues.Add(d1 * (i + 1));
                else
                {
                    int d2 = _dice[1];
                    _diceValues.Add(d2);
                    _diceValues.Add(d1 + d2);
                }
            }
        }

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }

        private void ShowAllowed()
        {
            _allowed.Clear();
            _model.ClearSelection();
            if (_dice.Count == 0 || _diceValues.Count == 0)
                return;

            if (_model[TW + _currentPlayer].Count > 0)
            {
                int from = TW + _currentPlayer;
                for (int i = 0; i < 6; i++)
                {
                    int to = White ? 23 - i : i;
                    if (_diceValues.Contains(i + 1) && (_model[to].Count <= 1 || _model[to].IsPlayerWhite == White))
                    {
                        _allowed.Add(from);
                        break;
                    }
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
                            {
                                _allowed.Add(from);
                                break;
                            }
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
                    int to = OW + _currentPlayer;
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
                                _allowed.Add(from);
                        }
                    }
                }
            }

            _allowed.Sort();
            foreach (var m in _allowed)
                _model[m].Select(true, true);
        }
    }
}
