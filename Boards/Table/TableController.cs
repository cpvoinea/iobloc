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
            if (_model[24 + _currentPlayer].Count > 0)
            {
            }

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
            ShowDice();
        }

        private void ShowDice()
        {
            _model.ShowDice(string.Join<int>(",", _dice).Split(','));
        }
    }
}
