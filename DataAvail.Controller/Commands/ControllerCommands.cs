using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Controllers.Commands;
using DataAvail.XObject;
using DataAvail.XObject.XContexts;
using DataAvail.XObject.XWP;
using System.Windows.Forms;

namespace DataAvail.Controllers
{
    partial class Controller
    {
        public class ControllerCommands
        {
            internal ControllerCommands(Controller Controller)
            {
                _controller = Controller;

                CreateDefaultCommands();
            }

            private readonly Controller _controller;

            private readonly List<IControllerCommand> _commands = new List<IControllerCommand>();

            private readonly List<IControllerCommandItem> _items = new List<IControllerCommandItem>();

            private Controller Controller
            {
                get { return _controller; }
            }

            private IEnumerable<IControllerCommand> Commands
            {
                get { return _commands; }
            }

            private IEnumerable<IControllerCommandItem> Items
            {
                get { return _items; }
            }

            private IEnumerable<DefaultControllerCommand> DefaultCommands
            {
                get { return Commands.OfType<DefaultControllerCommand>(); }
            }

            public void AddCommand(IControllerCommand Command)
            {
                Command.AvailableChanged += new EventHandler(command_AvailableChanged);

                Command.EnabledChanged += new EventHandler(command_EnabledChanged);

                _commands.Add(Command);
            }

            private void AddCommand(ControllerCommandTypes Type, Func<Controller, IControllerCommandItem, object> Action)
            {
                AddCommand(Type, Action, true);
            }

            private void AddCommand(ControllerCommandTypes Type, Func<Controller, IControllerCommandItem, object> Action, bool IsKeyDownCommand)
            {
                AddCommand(new DefaultControllerCommand(Type, Action, IsKeyDownCommand));
            }

            public void AddCommandItem(IControllerCommandItem CommandItem)
            {
                SetCommandItemAvaialble(CommandItem);

                SetCommandItemEnabled(CommandItem, false);

                _items.Add(CommandItem);

                CommandItem.Execute += new ControllerCommandItemExecuteHandler(CommandItem_Execute);
            }

            private bool IsCommandAvailable(IControllerCommandItem CommandItem)
            {
                IControllerCommand command = _commands.FirstOrDefault(p => p.CommandName == CommandItem.CommandName);

                if (command != null)
                {
                    return command.Available ? CheckFkCommandEnabled(CommandItem, false) : false;
                }
                else
                {
                    return false;
                }
            }

            private IControllerCommand GetCommand(IControllerCommandItem CommandItem)
            {
                return _commands.FirstOrDefault(p => p.CommandName == CommandItem.CommandName);
            }

            private bool IsCommandEnabled(IControllerCommandItem CommandItem, bool KeyCommand)
            {
                IControllerCommand command = GetCommand(CommandItem);//_commands.FirstOrDefault(p => p.CommandName == CommandItem.CommandName);

                if (command != null)
                {
                    return command.Enabled ? CheckFkCommandEnabled(CommandItem, KeyCommand) : false;
                }
                else
                {
                    return false;
                }
            }

            private bool CheckFkCommandEnabled(IControllerCommandItem CommandItem, bool IsKeyCommand)
            {
                if (Controller.TableContext.ActiveType == XOTableContextActiveType.Item)
                {
                    if (CommandItem.CommandName == ControllerCommandTypes.SelectFkItem.ToString())
                    {
                        return
                            DataAvail.Utils.EnumFlags.IsContain(((DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext)CommandItem.Context).fieldContext.FkInterfaceType,
                            IsKeyCommand ? XWPFieldFkInterfaceType.SelectItemKey : XWPFieldFkInterfaceType.SelectItemButton);
                    }
                    else if (CommandItem.CommandName == ControllerCommandTypes.AddFkItem.ToString())
                    {
                        return
                            DataAvail.Utils.EnumFlags.IsContain(((DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext)CommandItem.Context).fieldContext.FkInterfaceType,
                              IsKeyCommand ? XWPFieldFkInterfaceType.AddItemKey : XWPFieldFkInterfaceType.AddItemButton);
                    }
                }
                else
                {
                    if (CommandItem.CommandName == ControllerCommandTypes.SelectFkItem.ToString())
                    {
                        return
                            DataAvail.Utils.EnumFlags.IsContain(((DataAvail.Controllers.Commands.UICommandItems.FKCommandItemContext)CommandItem.Context).fieldContext.FkInterfaceType,
                            IsKeyCommand ? XWPFieldFkInterfaceType.SelectItemKeySearch : XWPFieldFkInterfaceType.SelectItemButtonSearch);
                    }
                    else if (CommandItem.CommandName == ControllerCommandTypes.AddFkItem.ToString())
                    {
                        return false;
                    }
                }

                return true;
            }



            public void AddCommandItems(IEnumerable<IControllerCommandItem> CommandItems)
            {
                foreach (IControllerCommandItem commandItem in CommandItems)
                    AddCommandItem(commandItem);
            }

            private IControllerCommand this[string CommandName]
            {
                get { return Commands.FirstOrDefault(p => p.CommandName == CommandName); }
            }

            private IControllerCommand this[ControllerCommandTypes CommandType]
            {
                get { return this[CommandType.ToString()]; }
            }


            private void CreateDefaultCommands()
            {
                AddCommand(ControllerCommandTypes.ItemShow, (p, x) => p.ItemShow(x));
                AddCommand(ControllerCommandTypes.ItemSelect, (p, x) => p.ItemSelect(x));
                AddCommand(ControllerCommandTypes.ListShow, (p, x) => p.ListShow());
                AddCommand(ControllerCommandTypes.ItemClose, (p, x) => p.ItemClose());
                AddCommand(ControllerCommandTypes.ListClose, (p, x) => p.ListClose());
                AddCommand(ControllerCommandTypes.EndEdit, (p, x) => p.CurrentEndEdit());
                AddCommand(ControllerCommandTypes.CancelEdit, (p, x) => p.CurrentCancelEdit());
                AddCommand(ControllerCommandTypes.AcceptChanges, (p, x) => p.CurrentAcceptChanges());
                AddCommand(ControllerCommandTypes.RejectChanges, (p, x) => p.CurrentRejectChanges());
                AddCommand(ControllerCommandTypes.Clone, (p, x) => p.CurrentClone());
                AddCommand(ControllerCommandTypes.Add, (p, x) => p.Add());
                AddCommand(ControllerCommandTypes.Remove, (p, x) => p.Remove());
                AddCommand(ControllerCommandTypes.MoveNext, (p, x) => p.CurrentMoveNext());
                AddCommand(ControllerCommandTypes.MovePerv, (p, x) => p.CurrentMovePerv());
                AddCommand(ControllerCommandTypes.BatchAcceptChanges, (p, x) => p.BatchAcceptChanges());
                AddCommand(ControllerCommandTypes.BatchRejectChanges, (p, x) => p.BatchRejectChanges());
                AddCommand(ControllerCommandTypes.Fill, (p, x) => p.Fill(), false);
                AddCommand(ControllerCommandTypes.SelectFkItem, (p, x) => p.itemSelector.SelectItem(x));
                AddCommand(ControllerCommandTypes.AddFkItem, (p, x) => p.itemSelector.AddItem(x));
                AddCommand(ControllerCommandTypes.UploadToExcel, (p, x) => p.UploadToExcel());
                AddCommand(ControllerCommandTypes.Refill, (p, x) => p.Refill(), false);
                AddCommand(ControllerCommandTypes.FocusSearch, (p, x) => p.FocusSearch());
                AddCommand(ControllerCommandTypes.FocusList, (p, x) => p.FocusList());
                
            }

            private bool Execute(IControllerCommand ControllerCommand)
            {
                return (bool)ControllerCommand.Action.Invoke(Controller, null);
            }

            public bool Execute(ControllerCommandTypes CommandType)
            {
                return Execute(this[CommandType.ToString()]);
            }

            void CommandItem_Execute(object sender, ControllerCommandItemExecuteEventArgs args)
            {
                IControllerCommandItem commandItem = (IControllerCommandItem)sender;

                args.result = this[commandItem.CommandName].Action.Invoke(Controller, commandItem);
            }

            private bool GetIsFieldDependedCommand(ControllerCommandTypes CommandType)
            {
                if (CommandType == ControllerCommandTypes.AddFkItem
                    || CommandType == ControllerCommandTypes.SelectFkItem)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            internal void RefreshAvailableDeafultCommands()
            {
                IEnumerable<ControllerCommandTypes> availCommandTypes = GetControllerCommandTypes(Controller.AvailableState);

                foreach (DefaultControllerCommand com in DefaultCommands)
                {
                    ///Here are some problems, commands could be fields independed, in this case the current implementation works well.
                    ///Depended fields are demanded some consideration, now the case implemented not quite well.

                    com.SetAvailable(availCommandTypes.Contains(com.commandType) ? true : false, GetIsFieldDependedCommand(com.commandType));

                }
            }

            internal void RefreshEnabledDefaultCommands()
            {
                IEnumerable<ControllerCommandTypes> enabledCommandTypes = GetControllerCommandTypes(Controller.State);

                foreach (DefaultControllerCommand com in DefaultCommands)
                {
                    com.SetEnabled(enabledCommandTypes.Contains(com.commandType) ? true : false, GetIsFieldDependedCommand(com.commandType));
                }
            }

            private IEnumerable<ControllerCommandTypes> GetControllerCommandTypes(ControllerStates ControllerState)
            {
                List<ControllerCommandTypes> list = new List<ControllerCommandTypes>();

                foreach (int commandType in DataAvail.Utils.EnumFlags.DecomposeEnum((int)ControllerState))
                {
                    list.AddRange(GetControllerCommandType((ControllerStates)commandType));
                }

                return list.Union(
                    new ControllerCommandTypes[] { ControllerCommandTypes.FocusList, ControllerCommandTypes.FocusSearch, ControllerCommandTypes.ItemClose });
            }


            private IEnumerable<ControllerCommandTypes> GetControllerCommandType(ControllerStates ControllerStates)
            {
                switch (ControllerStates)
                {
                    case ControllerStates.CanAdd:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.Add };

                    case ControllerStates.CanRemove:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.Remove };

                    case ControllerStates.CanEndEdit:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.EndEdit };

                    case ControllerStates.CanCancelEdit:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.CancelEdit };

                    case ControllerStates.CanAcceptChanges:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.AcceptChanges };

                    case ControllerStates.CanRejectChanges:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.RejectChanges };

                    case ControllerStates.CanClone:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.Clone };

                    case ControllerStates.CanMoveNext:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.MoveNext };

                    case ControllerStates.CanMovePerv:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.MovePerv };

                    case ControllerStates.ItemCanShow:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.ItemSelect, ControllerCommandTypes.ItemShow };

                    case ControllerStates.ListCanShow:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.ListShow };

                    case ControllerStates.CanBatchAcceptChanges:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.BatchAcceptChanges };

                    case ControllerStates.CanBatchRejectChanges:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.BatchRejectChanges };

                    case ControllerStates.CanFill:
                        return new ControllerCommandTypes[] 
                        {
                            ControllerCommandTypes.Fill, 
                            ControllerCommandTypes.UploadToExcel, 
                            ControllerCommandTypes.Refill,
                            ControllerCommandTypes.ListClose
                        };

                    case ControllerStates.CanEdit:
                        return new ControllerCommandTypes[] { ControllerCommandTypes.SelectFkItem, ControllerCommandTypes.AddFkItem };
                }

                return new ControllerCommandTypes[] { };
            }

            void command_EnabledChanged(object sender, EventArgs e)
            {
                IControllerCommand command = (IControllerCommand)sender;

                foreach (IControllerCommandItem commandItem in Items.Where(p => p.CommandName == command.CommandName))
                {
                    commandItem.Enabled = IsCommandEnabled(commandItem, false);
                }
            }

            void command_AvailableChanged(object sender, EventArgs e)
            {
                IControllerCommand command = (IControllerCommand)sender;

                foreach (IControllerCommandItem commandItem in Items.Where(p => p.CommandName == command.CommandName))
                {
                    SetCommandItemAvaialble(commandItem);
                }
            }

            private void SetCommandItemAvaialble(IControllerCommandItem CommandItem)
            {
                CommandItem.Available = IsCommandAvailable(CommandItem);
            }

            private void SetCommandItemEnabled(IControllerCommandItem CommandItem, bool KeyCommand)
            {
                CommandItem.Enabled = IsCommandEnabled(CommandItem, KeyCommand);
            }

            private object SelectCommand(object [] Commands)
            {
                IEnumerable<IControllerCommandItem> commandItems = Commands.OfType<IControllerCommandItem>();

                IEnumerable<IControllerCommand> commands = Commands.OfType<IControllerCommand>();

                if (commandItems.Count() > 0)
                {
                    //command items always more prioraized than commands
                    return commandItems.First();
                }
                else if (commands.Count() > 0)
                {
                    if (commands.Count() > 1)
                    {
                        if (commands.FirstOrDefault(p => p.CommandName == "CancelEdit" || p.CommandName == "EndEdit") != null && this.Controller.CurrentIsCanEndEdit)
                            return commands.First(p => p.CommandName == "CancelEdit" || p.CommandName == "EndEdit");

                        return commands.Where(p => p.CommandName != "EndEdit" && p.CommandName != "CancelEdit").First();
                    }
                    else
                    {
                        return commands.First();
                    }
                }

                return null;
            }

            #region Key handling


            internal bool HandleKey(System.Windows.Forms.Keys Modifyers, System.Windows.Forms.Keys Key, bool KeyDown)
            {
                object command = SelectCommand(GetCommandsByKey(Modifyers, Key).ToArray());

                if (command != null)
                {
                    DefaultControllerCommand defCommand = (command is IControllerCommand) ?
                        ((DefaultControllerCommand)command) : (DefaultControllerCommand)GetCommand((IControllerCommandItem)command);

                    if (defCommand.isKeyDownCommand == KeyDown)
                    {
                        if (command is IControllerCommand)
                        {
                            if (((IControllerCommand)command).Enabled)
                                Execute((IControllerCommand)command);

                            if (((IControllerCommand)command).CommandName == "ListClose")
                                return false;
                        }
                        else
                        {
                            if (IsCommandEnabled((IControllerCommandItem)command, true))
                                ((IControllerCommandItem)command).Exec();
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            private static bool CheckModifyers(string[] StrModifyers, System.Windows.Forms.Keys Modifyers)
            {
                return GetModifyers(StrModifyers) == Modifyers;
            }

            private static Keys GetModifyers(string[] StrModifyers)
            {
                Keys keyModifyers = Keys.None;

                if (StrModifyers != null)
                {
                    foreach (string str in StrModifyers)
                    {
                        switch (str.ToUpper())
                        {
                            case "ALT":
                                keyModifyers |= Keys.Alt;
                                break;
                            case "CTL":
                                keyModifyers |= Keys.Control;
                                break;
                            case "SHIFT":
                                keyModifyers |= Keys.Shift;
                                break;
                        }
                    }
                }

                return keyModifyers;
            }

            private IEnumerable<object> GetCommandsByKey(System.Windows.Forms.Keys Modifyers, System.Windows.Forms.Keys Key)
            {
                object command = null;

                foreach (XOKey key in Controller.TableContext.XOTable.XoApplication.GetKeys(ActiveKeyContext).Where(s => CheckModifyers(s.Modifyers, Modifyers) && s.Key.ToUpper() == Key.ToString().ToUpper()))
                {
                    switch (key.CommandType)
                    {
                        case XWPKeyCommandType.showItem: 
                            command = GetFocusedCommandItem(ControllerCommandTypes.ItemShow);
                            break;
                        case XWPKeyCommandType.selectItem: 
                            command = GetFocusedCommandItem(ControllerCommandTypes.ItemSelect);
                            break;
                        case XWPKeyCommandType.addItem: 
                            command = GetFocusedCommandItem(ControllerCommandTypes.Add);
                            break;
                        case XWPKeyCommandType.removeItem: 
                            command = GetFocusedCommandItem(ControllerCommandTypes.Remove);
                            break;
                        case XWPKeyCommandType.closeForm: 
                            command = this[IsListActive ? ControllerCommandTypes.ListClose : ControllerCommandTypes.ItemClose];
                            break;
                        case XWPKeyCommandType.saveChanges: 
                            command = this[IsListActive ? ControllerCommandTypes.BatchAcceptChanges : ControllerCommandTypes.AcceptChanges];
                            break;
                        case XWPKeyCommandType.rejectChanges: 
                            command = this[IsListActive ? ControllerCommandTypes.BatchRejectChanges : ControllerCommandTypes.RejectChanges];
                            break;
                        case XWPKeyCommandType.cancelEdit:
                            if (!IsListActive)
                                command = this[ControllerCommandTypes.CancelEdit];
                            break;
                        case XWPKeyCommandType.endEdit:
                            if (!IsListActive)
                                command = this[ControllerCommandTypes.EndEdit];
                            break;
                        case XWPKeyCommandType.movePerv: 
                            if (!IsListActive)
                                command = this[ControllerCommandTypes.MovePerv];
                            break;
                        case XWPKeyCommandType.moveNext: 
                            if (!IsListActive)
                                command = this[ControllerCommandTypes.MoveNext];
                            break;
                        case XWPKeyCommandType.fkAddItem: 
                            command = GetFocusedCommandItem(ControllerCommandTypes.AddFkItem);
                            break;
                        case XWPKeyCommandType.fkSelectItem: 
                            command = GetFocusedCommandItem(ControllerCommandTypes.SelectFkItem);
                            break;
                        case XWPKeyCommandType.refreshList: 
                            if (IsListActive)
                                command = this[ControllerCommandTypes.Refill];
                            break;
                        case XWPKeyCommandType.uploadToExcel: 
                            if (IsListActive)
                                command = this[ControllerCommandTypes.UploadToExcel];
                            break;
                        case XWPKeyCommandType.focusList: 
                            if (IsListActive)
                                command = this[ControllerCommandTypes.FocusList];
                            break;
                        case XWPKeyCommandType.focusSearch: 
                            if (IsListActive)
                                command = this[ControllerCommandTypes.FocusSearch];
                            break;
                        case XWPKeyCommandType.clone: 
                            if (!IsListActive)
                                command = this[ControllerCommandTypes.Clone];
                            break;
                    }

                    if (command != null)
                        yield return command;
                }

                //return null;
            }

            XWPKeyContextType ActiveKeyContext
            {
                get
                {
                    if (Controller.TableContext.IsFkItemContext)
                    {
                        return XWPKeyContextType.fkItem;
                    }
                    else
                    {
                        return XWPKeyContextType.undefined;
                    }
                }
            }

            private bool IsListActive
            {
                get
                {
                    return Controller.ListUI != null && Controller.ListUI.IsFocused;
                }
            }

            private IControllerCommandItem GetFocusedCommandItem(ControllerCommandTypes CommandType)
            {
                IXtraEditCommand editCommand = IsListActive ? Controller.ListUI.FocusedEditCommand : Controller.ItemUI.FocusedEditCommand;

                if (editCommand != null)
                {
                    IControllerCommandItem commandItem = editCommand.SupportedCommands.FirstOrDefault(p => p.CommandName == CommandType.ToString());

                    return commandItem;
                }
                else
                {
                    return null;
                }

            }

            #endregion
        }
    }
}
