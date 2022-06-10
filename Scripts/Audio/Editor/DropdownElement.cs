using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.Audio
{
    public class DropdownElement : VisualElement, INotifyValueChanged<string>
    {
        public string value
        {
            get => _menu.text == "UNSET" ? null : _menu.text;
            set => PopulateMenu(value);
        }

        private readonly ToolbarMenu _menu;

        public DropdownElement()
        {
            _menu = new ToolbarMenu();
            ((INotifyValueChanged<string>)_menu.ElementAt(0))
                .RegisterValueChangedCallback(evt => evt.StopImmediatePropagation());
            Add(_menu);
            PopulateMenu();
        }

        public void SetValueWithoutNotify(string newValue) => PopulateMenu(newValue);

        private void PopulateMenu(string text = null)
        {
            const string unset = "UNSET";
            _menu.text = text ?? unset;
            _menu.ClearMenu();

            // Unset action
            _menu.menu.AppendAction(unset, a =>
            {
                string oldValue = _menu.text == unset ? null : _menu.text;
                _menu.text = unset;
                CreateEvent(oldValue, null);
            });

            string projdir = Directory.GetCurrentDirectory();
            string libdir = $"{projdir}/{NMDatabase.DBPath}/Sound-Library";
            if (Directory.Exists(libdir)) LibraryScan(libdir);

            void LibraryScan(string curdir)
            {
                string soundParent = Path.GetFileName(curdir) == "Sound-Library" ?
                    null : curdir.Replace(libdir + '/', "");

                // All option
                string name = soundParent == null ? "ALL" : $"{soundParent}/ALL";
                _menu.menu.AppendAction(name, a =>
                {
                    string oldValue = _menu.text == unset ? null : _menu.text;
                    CreateEvent(oldValue, name);
                    _menu.text = name;
                });

                foreach (string path in Directory.GetFiles(curdir))
                {
                    if (Path.GetExtension(path) == ".meta") continue;
                    string file = Path.GetFileNameWithoutExtension(path);
                    string fileName = soundParent == null ? file : $"{soundParent}/{file}";

                    _menu.menu.AppendAction(fileName, a =>
                    {
                        string oldValue = _menu.text == unset ? null : _menu.text;
                        CreateEvent(oldValue, fileName);
                        _menu.text = fileName;
                    });
                }

                foreach (string dir in Directory.GetDirectories(curdir))
                {
                    LibraryScan($"{curdir}/{Path.GetFileName(dir)}");
                }
            }

            void CreateEvent(string oldValue, string newValue)
            {
                using ChangeEvent<string> evt = ChangeEvent<string>.GetPooled(oldValue, newValue);
                evt.target = this;
                SendEvent(evt);
            }
        }
    }
}
