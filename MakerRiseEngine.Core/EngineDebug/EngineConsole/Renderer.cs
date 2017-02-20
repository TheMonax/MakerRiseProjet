﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maker.RiseEngine.Core.Rendering;
using Maker.RiseEngine.Core.Input;
using Microsoft.Xna.Framework.Input;

namespace Maker.RiseEngine.Core.EngineDebug.EngineConsole
{
    class Renderer : IDrawable
    {
        enum State
        {
            Opened,
            Opening,
            Closed,
            Closing
        }

        public bool IsOpen
        {
            get
            {
                return currentState == State.Opened;
            }
        }

        private readonly SpriteBatch spriteBatch;
        private readonly InputProcessor inputProcessor;
        private State currentState;
        private Vector2 openedPosition, closedPosition, position;
        private DateTime stateChangeTime;
        private Vector2 firstCommandPositionOffset;
        private Vector2 FirstCommandPosition
        {
            get
            {
                return new Vector2(InnerBounds.X, InnerBounds.Y) + firstCommandPositionOffset;
            }
        }

        Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, GameConsoleOptions.Options.Width - (GameConsoleOptions.Options.Margin * 2), GameConsoleOptions.Options.Height);
            }
        }

        Rectangle InnerBounds
        {
            get
            {
                return new Rectangle(Bounds.X + GameConsoleOptions.Options.Padding, Bounds.Y + GameConsoleOptions.Options.Padding, Bounds.Width - GameConsoleOptions.Options.Padding, Bounds.Height);
            }
        }

        private readonly float oneCharacterWidth;
        private int maxCharactersPerLine;

        public Renderer(Game game, SpriteBatch spriteBatch, InputProcessor inputProcessor)
        {
            currentState = State.Closed;
            position = closedPosition = new Vector2(-GameConsoleOptions.Options.Width, 0);
            openedPosition = new Vector2(0, 0);
            this.spriteBatch = spriteBatch;
            this.inputProcessor = inputProcessor;
            firstCommandPositionOffset = Vector2.Zero;
            oneCharacterWidth = GameConsoleOptions.Options.Font.MeasureString("x").X;
            maxCharactersPerLine = (int)((Bounds.Width - GameConsoleOptions.Options.Padding * 2) / oneCharacterWidth);
        }

     

        void DrawCursor(Vector2 pos, GameTime gameTime)
        {
            if (!IsInBounds(pos.Y))
            {
                return;
            }
            var split = SplitCommand(inputProcessor.Buffer.ToString(), maxCharactersPerLine).Last();
            pos.X += GameConsoleOptions.Options.Font.MeasureString(split).X;
            pos.Y -= GameConsoleOptions.Options.Font.LineSpacing;
            spriteBatch.DrawString(GameConsoleOptions.Options.Font, (int)(gameTime.TotalGameTime.TotalSeconds / GameConsoleOptions.Options.CursorBlinkSpeed) % 2 == 0 ? GameConsoleOptions.Options.Cursor.ToString() : "", pos, GameConsoleOptions.Options.CursorColor);
        }

        /// <summary>
        /// Draws the specified command and returns the position of the next command to be drawn
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        Vector2 DrawCommand(string command, Vector2 pos, Color color)
        {
            var splitLines = command.Length > maxCharactersPerLine ? SplitCommand(command, maxCharactersPerLine) : new[] { command };
            foreach (var line in splitLines)
            {
                if (IsInBounds(pos.Y))
                {
                    spriteBatch.DrawString(GameConsoleOptions.Options.Font, line, pos, color);
                }
                ValidateFirstCommandPosition(pos.Y + GameConsoleOptions.Options.Font.LineSpacing);
                pos.Y += GameConsoleOptions.Options.Font.LineSpacing;
            }
            return pos;
        }

        static IEnumerable<string> SplitCommand(string command, int max)
        {
            var lines = new List<string>();
            while (command.Length > max)
            {
                var splitCommand = command.Substring(0, max);
                lines.Add(splitCommand);
                command = command.Substring(max, command.Length - max);
            }
            lines.Add(command);
            return lines;
        }

        /// <summary>
        /// Draws the specified collection of commands and returns the position of the next command to be drawn
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        Vector2 DrawCommands(IEnumerable<OutputLine> lines, Vector2 pos)
        {
            var originalX = pos.X;
            foreach (var command in lines)
            {
                if (command.Type == OutputLineType.Command)
                {
                    pos = DrawPrompt(pos);
                }
                //position.Y = DrawCommand(command.ToString(), position, GameConsoleOptions.Options.FontColor).Y;
                pos.Y = DrawCommand(command.ToString(), pos, command.Type == OutputLineType.Command ? GameConsoleOptions.Options.PastCommandColor : GameConsoleOptions.Options.PastCommandOutputColor).Y;
                pos.X = originalX;
            }
            return pos;
        }

        /// <summary>
        /// Draws the prompt at the specified position and returns the position of the text that will be drawn next to it
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Vector2 DrawPrompt(Vector2 pos)
        {
            spriteBatch.DrawString(GameConsoleOptions.Options.Font, GameConsoleOptions.Options.Prompt, pos, GameConsoleOptions.Options.PromptColor);
            pos.X += oneCharacterWidth * GameConsoleOptions.Options.Prompt.Length + oneCharacterWidth;
            return pos;
        }

        public void Open()
        {
            if (currentState == State.Opening || currentState == State.Opened)
            {
                return;
            }
            stateChangeTime = DateTime.Now;
            currentState = State.Opening;
        }

        public void Close()
        {
            if (currentState == State.Closing || currentState == State.Closed)
            {
                return;
            }
            stateChangeTime = DateTime.Now;
            currentState = State.Closing;
        }

        void ValidateFirstCommandPosition(float nextCommandY)
        {
            if (!IsInBounds(nextCommandY))
            {
                firstCommandPositionOffset.Y -= GameConsoleOptions.Options.Font.LineSpacing;
            }
        }

        bool IsInBounds(float yPosition)
        {
            return yPosition < openedPosition.Y + GameConsoleOptions.Options.Height;
        }

        public void Update(GameInput playerInput, GameTime gameTime)
        {
            closedPosition = new Vector2(-GameConsoleOptions.Options.Width, 0);
            maxCharactersPerLine = (int)((Bounds.Width - GameConsoleOptions.Options.Padding * 2) / oneCharacterWidth);
            if (currentState == State.Opening)
            {
                position.X = MathHelper.SmoothStep(position.X, openedPosition.X, ((float)((DateTime.Now - stateChangeTime).TotalSeconds / GameConsoleOptions.Options.AnimationSpeed)));
                if (position.X == openedPosition.X)
                {
                    currentState = State.Opened;
                }
            }
            if (currentState == State.Closing)
            {
                position.X = MathHelper.SmoothStep(position.X, closedPosition.X, ((float)((DateTime.Now - stateChangeTime).TotalSeconds / GameConsoleOptions.Options.AnimationSpeed)));
                if (position.X == closedPosition.X)
                {
                    currentState = State.Closed;
                }
            }

            if (currentState == State.Opened)
            {

                if (playerInput.IsKeyBoardKeyPress(Keys.PageUp)){

                    firstCommandPositionOffset.Y -= 16;

                }

                if (playerInput.IsKeyBoardKeyPress(Keys.PageDown)){

                    firstCommandPositionOffset.Y += 16;

                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (currentState == State.Closed) //Do not draw if the console is closed
            {
                return;
            }
            spriteBatch.FillRectangle(Bounds, GameConsoleOptions.Options.BackgroundColor);
            spriteBatch.DrawRectangle(Bounds, Color.Black);

            var nextCommandPosition = DrawCommands(inputProcessor.Out, FirstCommandPosition);
            nextCommandPosition = DrawPrompt(nextCommandPosition);
            var bufferPosition = DrawCommand(inputProcessor.Buffer.ToString(), nextCommandPosition, GameConsoleOptions.Options.BufferColor); //Draw the buffer
            DrawCursor(bufferPosition, gameTime);
        }
    }
}