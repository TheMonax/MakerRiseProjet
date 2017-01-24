﻿using Maker.RiseEngine.Core.Content;
using Maker.RiseEngine.Core.Input;
using Maker.RiseEngine.Core.Rendering;
using Maker.RiseEngine.Core.Scenes;
using Maker.RiseEngine.Core.UserInterface;
using Maker.RiseEngine.Core.UserInterface.Controls;
using Maker.twiyol.Game.GameUtils;
using Maker.twiyol.Generator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace Maker.twiyol.Scenes.Menu
{
    public class MenuNewWorld : Scene
    {
        Panel rootContainer;
        Panel controlContainer;

        Button createNewWorldButton;
        Button goBackButton;

        TextBox worldNameTexBox;
        TextBox worldSeedTextBox;

        Label titleLabel;
        Label nameLabel;
        Label seedLabel;

        public override void OnLoad()
        {

            rootContainer = new Panel(new Rectangle(-350, -250, 700, 500), Color.Transparent);
            rootContainer.Padding = new ControlPadding(16);
            rootContainer.ControlAnchor = Anchor.Center;

            controlContainer = new Panel(new Rectangle(0, 0, 0, 96), Color.White);
            controlContainer.Padding = new ControlPadding(16);
            controlContainer.ControlDock = Dock.Bottom;

            createNewWorldButton = new Button("Créer le nouveau monde", new Rectangle(0, 0, 400, 64), Color.White);
            createNewWorldButton.ControlDock = Dock.Right;
            createNewWorldButton.onMouseClick += CreateNewWorldButton_onMouseClick;

            goBackButton = new Button("Retour", new Rectangle(0, 0, 200, 64), Color.White);
            goBackButton.ControlDock = Dock.Left;
            goBackButton.onMouseClick += GoBackButton_onMouseClick;

            worldNameTexBox = new TextBox("Monde sans nom", new Rectangle(0, 0, 128, 64), Color.White, Color.Black);
            worldNameTexBox.ControlDock = Dock.Top;
            worldSeedTextBox = new TextBox(new Random().Next().ToString(), new Rectangle(0, 0, 128, 64), Color.White, Color.Black);
            worldSeedTextBox.ControlDock = Dock.Top;

            titleLabel = new Label("Créer un nouveau monde", new Rectangle(0, 0, 128, 96), Color.White);
            titleLabel.ControlDock = Dock.Top;
            titleLabel.TextStyle = Style.rectangle;
            titleLabel.TextFont = ContentEngine.SpriteFont("Engine", "Bebas_Neue_48pt");

            nameLabel = new Label("Nom du nouveau monde :", new Rectangle(0, 0, 128, 64), Color.White);
            nameLabel.ControlDock = Dock.Top;
            seedLabel = new Label("Graine pour la génération du monde :", new Rectangle(0, 0, 128, 64), Color.White);
            seedLabel.ControlDock = Dock.Top;

            rootContainer.AddChild(controlContainer);
            rootContainer.AddChild(titleLabel);
            rootContainer.AddChild(nameLabel);
            rootContainer.AddChild(worldNameTexBox);
            rootContainer.AddChild(seedLabel);
            rootContainer.AddChild(worldSeedTextBox);

            controlContainer.AddChild(createNewWorldButton);
            controlContainer.AddChild(goBackButton);
        }

        private void CreateNewWorldButton_onMouseClick()
        {
            ThreadStart GenHandle = new ThreadStart(delegate
            {
                RiseEngine.sceneManager.RemoveScene(this);

                GeneratorProperty generatorProperty = new GeneratorProperty(worldNameTexBox.Text, int.Parse(worldSeedTextBox.Text));

                WorldGenerator Gen = new WorldGenerator(generatorProperty);
                var world = Gen.Generate();
                Game.GameScene wrldsc = new Game.GameScene(world);

                RiseEngine.sceneManager.AddScene(wrldsc);

                wrldsc.show();
                RiseEngine.sceneManager.RemoveScene(this);
            });

            Thread t = new Thread(GenHandle);
            t.Start();
        }

        private void GoBackButton_onMouseClick()
        {
            Scene menu = new Menu.MenuMain();
            RiseEngine.sceneManager.AddScene(menu);
            menu.show();
            RiseEngine.sceneManager.RemoveScene(this);
        }

        public override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            rootContainer.Draw(spriteBatch, gameTime);

        }

        public override void OnUnload()
        {



        }

        public override void OnUpdate(GameInput playerInput, GameTime gameTime)
        {

            rootContainer.Update(playerInput, gameTime);

        }

    }
}
