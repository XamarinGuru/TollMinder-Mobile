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

        private LabelForDataWheel wheelField;
        private List<T> elements;

        public PickerModel(LabelForDataWheel wheelField, List<T> elementsList)//TextFieldValidationWithImage
        {
            this.wheelField = wheelField;
            elements = elementsList;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return elements != null ? elements.Count : 0;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            switch (component)
            {
                case 0:
                return elements[(int)row].ToString();
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (elements != null)
            {
                wheelField.WheelText.Text = String.Format("{0}", //textField.TextFieldWithValidator.TextField.Text = String.Format("{0}",
                    elements[(int)pickerView.SelectedRowInComponent(0)]);
                pickerView.Hidden = true;
            }
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
