using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Linq;
using TempoIDE.Classes;

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
            var commands = XmlLoader.Get("app.commands.edit");

            foreach (var command in commands.Root.Elements("command"))
            {
                var name = command.Attribute("Name")?.Value;
                var keybinds = command.Element("Keybind");

                if (name is null)
                {
                    Console.WriteLine($@"WARNING: Xml attribute 'name' is not valid on element '{command.Name}' (in app.commands.edit.xml)");
                    continue;
                }

                if (keybinds is null)
                {
                    Console.WriteLine($@"WARNING: Xml attribute 'Keybind' is not valid on element '{name}' (in app.commands.edit.xml)");
                    return;
                }
                
                Commands.Add(
                    new AppCommand(name, ParseKeybindFromXElement(keybinds)
                ));
            }
        }

        private Keybind ParseKeybindFromXElement(XElement xml)
        {
            var keys = new List<Key>();
            
            foreach (var key in xml.Elements())
            {
                if (Enum.TryParse(key.Value, out Key parsed))
                    keys.Add(parsed);
                else
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