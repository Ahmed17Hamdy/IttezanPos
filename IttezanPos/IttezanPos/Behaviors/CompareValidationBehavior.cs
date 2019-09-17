using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IttezanPos.Behaviors
{
    public class CompareValidationBehavior : Behavior<Entry>
    {

        public static BindableProperty TextProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            BindableProperty.Create<CompareValidationBehavior, string>(tc => tc.Text, string.Empty, BindingMode.TwoWay);
#pragma warning restore CS0618 // Type or member is obsolete

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }


        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            IsValid = e.NewTextValue == Text;

            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
    public class CompareValidationBehaviorGreater : Behavior<Entry>
    {

        public static BindableProperty TextProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            BindableProperty.Create<CompareValidationBehaviorGreater, int>(tc => tc.Text, 0, BindingMode.TwoWay);
#pragma warning restore CS0618 // Type or member is obsolete

        public int Text
        {
            get
            {
                return (int)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }


        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            if (e.NewTextValue != "")
            {
                IsValid = int.Parse(e.NewTextValue) >= Text;

                ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
            }
           
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
