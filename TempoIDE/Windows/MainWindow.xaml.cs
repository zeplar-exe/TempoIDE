using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Linq;
using TempoIDE.Classes;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        private List<AppCommand> Commands = new List<AppCommand>();
        
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
            var menus = XmlLoader.Get("app.commands");

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

                    var keybind = ParseKeybindFromXElement(keybinds);

                    newMenu.Items.Add(new MenuItem { Header = commandName, InputGestureText = keybind.ToString()/* TODO: Keybinds */ });

                    Commands.Add(new AppCommand(commandName, keybind));
                }

                TopbarMenu.Children.Add(newMenu);
            }
        }

        private Keybind ParseKeybindFromXElement(XElement xml)
        {
            var keys = new List<Key>();
            
            if (xml is null)
                return new Keybind(keys.ToArray());
            
            foreach (var key in xml.Elements())
            {
                if (Enum.TryParse(key.Value, out Key parsed))
                    keys.Add(parsed);
                else if (key.Value != "None")
                    Console.WriteLine($@"WARNING: Key '{key.Value}' is not valid.");
            }

            return new Keybind(keys.ToArray());
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            foreach (var command in Commands)
            {
                if (command.Keybind.IsPressed())
                {
                    AppCommands.FromAppCommand(command, this);

                    e.Handled = true;
                    break;
                }
            }
        }
    }
}