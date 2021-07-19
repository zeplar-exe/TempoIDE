using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using TempoIDE.Classes;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Editor.TextWriter();
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var menus = ResourceCache.GetXml("app.commands");

            foreach (var menu in menus.Root.Elements("menu"))
            {
                var menuName = menu.Attribute("Name")?.Value;
                var commands = menu.Elements();
                
                var newMenu = new TopbarMenu { MenuName = menuName ?? "Menu" };

                foreach (var command in commands)
                {
                    if (command.Name == "divider")
                    {
                        newMenu.Items.Add(new ContextMenuSeparator());
                        continue;
                    }

                    var commandName = command.Attribute("Name")?.Value;
                    var keybinds = command.Element("Keybind");

                    if (commandName is null)
                    {
                        Console.WriteLine($@"WARNING: Xml attribute 'Name' is not valid on element '{command.Name}' (in app.commands.xml)");
                        commandName = "Command";
                    }
                    
                    var keybind = ParseKeybindingFromXElement(keybinds);
                    var routedCommand = (ICommand) AppCommands.FromName(commandName) ?? new RoutedCommand();
                    
                    newMenu.Items.Add(new MenuItem
                    {
                        Header = commandName,
                        CommandParameter = this,
                        Command = routedCommand,
                        InputGestureText = keybind.ToString()
                    });

                    var binding = new KeyBinding(routedCommand, new KeyGesture(keybind.Key, keybind.Modifiers))
                    {
                        CommandParameter = this
                    };
                    
                    InputBindings.Add(binding);
                }

                TopbarMenu.Children.Add(newMenu);
            }
        }

        private Keybind ParseKeybindingFromXElement(XElement xml)
        {
            var singleKey = Key.None;
            var modifiers = ModifierKeys.None;

            if (xml is null)
                return new Keybind(singleKey, modifiers);
            
            foreach (var key in xml.Elements())
            {
                if (key.Name == "Key" && Enum.TryParse(key.Value, out Key parsedKey))
                    singleKey = parsedKey;
                else if (key.Name == "Control" && Enum.TryParse(key.Value, out ModifierKeys parsedModifier))
                    modifiers |= parsedModifier;
                else if (key.Value != "None")
                    Console.WriteLine($@"WARNING: Key '{key.Value}' is not valid.");
            }

            return new Keybind(singleKey, modifiers);
        }

        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenFileEventArgs e)
        {
            Editor.OpenFile(e.NewFile);
        }

        private void Editor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        private void Explorer_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested); // TODO: Doesn't work at all
        }
    }
}