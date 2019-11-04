using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IttezanPos.CustomControls
{
    public class CustomStepper : StackLayout
    {
        Button PlusBtn;
        Button MinusBtn;
        Entry Entry;

        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(int),
              declaringType: typeof(CustomStepper),
              defaultValue: 1,
              defaultBindingMode: BindingMode.TwoWay);

        public int Text
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public CustomStepper()
        {
            PlusBtn = new Button { Text = "+", WidthRequest = 40, FontAttributes = FontAttributes.Bold, FontSize = 15 };
            MinusBtn = new Button { Text = "-", WidthRequest = 40, FontAttributes = FontAttributes.Bold, FontSize = 15 };
            switch (Device.RuntimePlatform)
            {

                case Device.UWP:
                case Device.Android:
                    {
                        PlusBtn.BackgroundColor = Color.Transparent;
                        MinusBtn.BackgroundColor = Color.Transparent;
                        break;
                    }
                case Device.iOS:
                    {
                        PlusBtn.BackgroundColor = Color.DarkGray;
                        MinusBtn.BackgroundColor = Color.DarkGray;
                        break;
                    }
            }

            Orientation = StackOrientation.Horizontal;
            PlusBtn.Clicked += PlusBtn_Clicked;
            MinusBtn.Clicked += MinusBtn_Clicked;
            Entry = new Entry
            {
                PlaceholderColor = Color.Gray,
                HorizontalTextAlignment = TextAlignment.Center,
                IsEnabled = false,
                Keyboard = Keyboard.Numeric,
                WidthRequest = 40,
                BackgroundColor = Color.FromHex("#3FFF")
            };
            Entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), BindingMode.TwoWay, source: this));
            Entry.TextChanged += Entry_TextChanged;
            Children.Add(PlusBtn);
            Children.Add(Entry);
            Children.Add(MinusBtn);
        }
        private EventHandler onValueChangedEvent = null;

        public event EventHandler OnValueChanged
        {
            add
            {
                onValueChangedEvent = null;
                onValueChangedEvent = value;
            }
            remove
            {
                // Will show a warning. You can ignore it.
                onValueChangedEvent = null;
            }
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
                this.Text = int.Parse(e.NewTextValue);
            onValueChangedEvent?.Invoke(this, e);
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (Text > 1)
                Text--;
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            Text++;
        }

    }

}
