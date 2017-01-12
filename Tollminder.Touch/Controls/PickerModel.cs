using System;
using System.Collections.Generic;
using UIKit;

namespace Tollminder.Touch.Controls
{
    public class PickerModel : UIPickerViewModel
    {
        static string[] names = new string[] {
            "pscorlib.dll",
            "pscorlib_aot.dll",
            "Mono.PlayScript.dll",
            "PlayScript.Dynamic.dll",
            "PlayScript.Dynamic_aot.dll",
            "PlayScript.Optimization.dll",
            "playshell.exe",
            "psc.exe"
        };

        private UILabel textField;
        private string[] elements;

        public PickerModel(UILabel textField, string[] elementsList)//TextFieldValidationWithImage
        {
            this.textField = textField;
            elements = elementsList;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return names.Length;//elements.Length;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            switch (component)
            {
                case 0:
                    return names[row];
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            textField.Text = String.Format("{0}", //textField.TextFieldWithValidator.TextField.Text = String.Format("{0}",
                names[pickerView.SelectedRowInComponent(0)]);
            pickerView.Hidden = true;
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            if (component == 0)
                return 220f;
            else
                return 30f;
        }
    }
}
