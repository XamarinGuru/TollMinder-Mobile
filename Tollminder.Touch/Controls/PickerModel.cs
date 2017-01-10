using System;
using System.Collections.Generic;
using UIKit;

namespace Tollminder.Touch.Controls
{
    public class PickerModel<T> : UIPickerViewModel
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

        private TextFieldValidationWithImage textField;
        private string[] elements;

        public PickerModel(TextFieldValidationWithImage textField, string[] elementsList)
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
            return elements.Length;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            switch (component)
            {
                case 0:
                    return elements[row];
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            textField.TextFieldWithValidator.TextField.Text = String.Format("{0}",
                elements[pickerView.SelectedRowInComponent(0)]);
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
