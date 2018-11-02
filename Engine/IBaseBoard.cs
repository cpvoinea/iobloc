namespace iobloc
{
    interface IBaseBoard : IBoard
    {
        int Score { get; set; }
        int Level { get; set; }
        IBaseBoard Next { get; }

        void InitializeUI();
        void Initialize();
        void Paint();
        void Change(bool set);
    }
}
