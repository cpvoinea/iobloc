using System;

namespace iobloc
{
    public delegate void LoopHandler();

    public interface IRenderer : IDisposable
    {
        void DrawBorder(Border border);
        // Summary:
        //      Draw a pane inside a rectangular area.
        //      The pane has either lines of text or a multi-colored matrix with a single character
        // Parameters: pane: pane to draw
        void DrawPane(Pane pane);
        // Summary:
        //      Wait until key is pressed and return key
        string InputWait();
        // Summary:
        //      Check if key is pressed and return it, return null if no key is pressed
        string Input();
        void StartLoop(int interval);
        void StopLoop();
        event LoopHandler NextInLoop;
    }
}
