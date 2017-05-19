using Reveil.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Reveil.Controls
{
    /// <summary>
    /// Time Picker as a control that lets the user select a specific time
    /// </summary>
    [TemplatePart(Name = HourPart, Type = typeof(TextBox)),
    TemplatePart(Name = MinutePart, Type = typeof(TextBox)),
    TemplatePart(Name = SecondPart, Type = typeof(TextBox)),
    TemplatePart(Name = IncreaseTimePart, Type = typeof(ButtonBase)),
    TemplatePart(Name = DecrementTimePart, Type = typeof(ButtonBase))]
    public class TimePicker : Control
    {

        #region Champs
        private const string HourPart = "PART_Hours";
        private const string MinutePart = "PART_Minutes";
        private const string SecondPart = "PART_Seconds";
        private const string IncreaseTimePart = "PART_IncreaseTime";
        private const string DecrementTimePart = "PART_DecrementTime";

        public static readonly DependencyProperty MaxTimeProperty =
            DependencyProperty.Register(
                nameof(MaxTime),
                typeof(TimeSpan),
                typeof(TimePicker),
                new UIPropertyMetadata(TimeSpan.MaxValue, TimePicker_LimitsChanged));
        public static readonly DependencyProperty MinTimeProperty =
            DependencyProperty.Register(
                nameof(MinTime),
                typeof(TimeSpan),
                typeof(TimePicker),
                new UIPropertyMetadata(TimeSpan.MinValue, TimePicker_LimitsChanged));
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register(
                nameof(SelectedTime),
                typeof(TimeSpan),
                typeof(TimePicker),
                new UIPropertyMetadata(new TimeSpan(0, 0, 0), SelectedTimePropertyChanged, ForceValidSelectedTime));
        public static readonly DependencyProperty SelectedHourProperty =
            DependencyProperty.Register(
                nameof(SelectedHour),
                typeof(int),
                typeof(TimePicker),
                new UIPropertyMetadata(0, TimePicker_HourChanged));
        public static readonly DependencyProperty SelectedMinuteProperty =
            DependencyProperty.Register(
                nameof(SelectedMinute),
                typeof(int),
                typeof(TimePicker),
                new UIPropertyMetadata(0, TimePicker_MinuteChanged));
        public static readonly RoutedEvent SelectedTimeChangedEvent =
             EventManager.RegisterRoutedEvent(
                 nameof(SelectedTimeChanged),
                 RoutingStrategy.Bubble,
                 typeof(TimeSelectedChangedEventHandler),
                 typeof(TimePicker));
        public static readonly DependencyProperty SelectedSecondProperty =
            DependencyProperty.Register(nameof(SelectedSecond),
                typeof(int),
                typeof(TimePicker),
                new UIPropertyMetadata(0, TimePicker_SecondChanged));

        private int hourMaxValue = 23;
        private int minuteMaxValue = 59;
        private int secondMaxValue = 59;
        private int hourMinValue;
        private int minuteMinValue = 0;
        private int secondMinValue = 0;
        //data memebers to store the textboxes for hours, minutes and seconds
        private TextBox hours, minutes, seconds;
        //the textbox that is selected
        private TextBox currentlySelectedTextBox;
        private bool isUpdatingTime = false;
        #endregion

        #region Constructeurs
        /// <summary>
        /// Static constructor
        /// </summary>
        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TimePicker), 
                new FrameworkPropertyMetadata(typeof(TimePicker)));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimePicker()
        {
            SelectedTime = DateTime.Now.TimeOfDay;
        }
        #endregion

        #region Délégués
        /// <summary>
        /// Delegate for the TimeSelectedChanged event
        /// </summary>
        /// <param name="sender">The object raising the event</param>
        /// <param name="e">The routed event arguments</param>
        public delegate void TimeSelectedChangedEventHandler(object sender, TimeSelectedChangedRoutedEventArgs e);
        #endregion

        #region Evenements
        public event TimeSelectedChangedEventHandler SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Gets or sets the maximum time that can be selected
        /// </summary>
        public TimeSpan MaxTime
        {
            get { return (TimeSpan)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum time that can be selected
        /// </summary>
        public TimeSpan MinTime
        {
            get { return (TimeSpan)GetValue(MinTimeProperty); }
            set { SetValue(MinTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected Hour
        /// </summary>
        public int SelectedHour
        {
            get { return (int)GetValue(SelectedHourProperty); }
            set { SetValue(SelectedHourProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected minutes
        /// </summary>
        public int SelectedMinute
        {
            get { return (int)GetValue(SelectedMinuteProperty); }
            set { SetValue(SelectedMinuteProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected second
        /// </summary>
        public int SelectedSecond
        {
            get { return (int)GetValue(SelectedSecondProperty); }
            set { SetValue(SelectedSecondProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected timestamp 
        /// </summary>
        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Exposes TryFocusNeighbourControl
        /// </summary>
        /// <param name="currentControl"></param>
        /// <param name="leftControl"></param>
        /// <param name="rightControl"></param>
        /// <param name="keyPressed"></param>
        public static void ExposeTryFocusNeighbourControl(TextBox currentControl, TextBox leftControl, TextBox rightControl, Key keyPressed)
        {
            TryFocusNeighbourControl(currentControl, leftControl, rightControl, keyPressed);
        }
        /// <summary>
        /// Exposes the AdjustCarretIndexOrMoveToNeighbour
        /// </summary>
        /// <param name="current"></param>
        /// <param name="neighbour"></param>
        public static void ExposeAdjustCarretIndexOrMoveToNeighbour(TextBox current, TextBox neighbour)
        {
            AdjustCarretIndexOrMoveToNeighbour(current, neighbour);
        }

        /// <summary>
        /// Exposes the TrimSelectedText method
        /// </summary>
        /// <param name="textBox"></param>
        public static void ExposeTrimSelectedText(TextBox textBox)
        {
            TrimSelectedText(textBox);
        }

        /// <summary>
        /// override to hook to the Control template elements
        /// </summary>
        public override void OnApplyTemplate()
        {
            //get the hours textbox and hook the events to it
            hours = GetTemplateChild(HourPart) as TextBox;
            hours.PreviewTextInput += TextBox_HourTextChanged;
            hours.KeyUp += HoursKeyUp;
            hours.GotFocus += TextGotFocus;
            hours.GotMouseCapture += TextGotFocus;

            //get the minutes textbox and hook the events to it
            minutes = GetTemplateChild(MinutePart) as TextBox;
            minutes.PreviewTextInput += TextBox_MinuteTextChanged;
            minutes.KeyUp += MinutesKeyUp;
            minutes.GotFocus += TextGotFocus;
            minutes.GotMouseCapture += TextGotFocus;

            //get the seconds textbox and hook the events to it
            seconds = GetTemplateChild(SecondPart) as TextBox;
            seconds.PreviewTextInput += TextBox_SecondTextChanged;
            seconds.KeyUp += SecondsKeyUp;
            seconds.GotFocus += TextGotFocus;
            seconds.GotMouseCapture += TextGotFocus;

            //Get the increase button and hook to the click event
            ButtonBase increaseButton = GetTemplateChild(IncreaseTimePart) as ButtonBase;
            increaseButton.Click += IncreaseTime;
            //Get the decrease button and hook to the click event
            ButtonBase decrementButton = GetTemplateChild(DecrementTimePart) as ButtonBase;
            decrementButton.Click += DecrementTime;
        }

        private static void AdjustCarretIndexOrMoveToNeighbour(TextBox current, TextBox neighbour)
        {
            //if the current is near the end move to neighbour
            if (current.CaretIndex == 1 && neighbour != null)
                neighbour.Focus();

            //if the carrot is in the first index move the caret one index
            else if (current.CaretIndex == 0)
                current.CaretIndex++;
        }

        private static string AdjustText(TextBox textBox, string newText)
        {
            //replace the new text with the old text if there are already 2 char in the textbox
            if (textBox.Text.Length == 2)
            {
                if (textBox.CaretIndex == 0)
                    return newText + textBox.Text[1];
                else
                    return textBox.Text[0] + newText;
            }
            else
            {
                return textBox.CaretIndex == 0 ?
                    newText + textBox.Text //if the carrot is in front the text append the new text infront
                    : textBox.Text + newText; //else put it in behind the existing text
            }

        }

        private static object ForceValidSelectedTime(DependencyObject sender, object value)
        {
            TimePicker picker = (TimePicker)sender;
            TimeSpan time = (TimeSpan)value;
            if (time < picker.MinTime)
                return picker.MinTime;
            if (time > picker.MaxTime)
                return picker.MaxTime;
            return time;
        }

        private static void SelectedTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TimePicker timePicker = (TimePicker)sender;
            TimeSpan newTime = (TimeSpan)e.NewValue;
            TimeSpan oldTime = (TimeSpan)e.OldValue;

            if (!timePicker.isUpdatingTime)
            {
                timePicker.BeginUpdateSelectedTime();//signal that the selected time is being updated

                if (timePicker.SelectedHour != newTime.Hours)
                    timePicker.SelectedHour = newTime.Hours;

                if (timePicker.SelectedMinute != newTime.Minutes)
                    timePicker.SelectedMinute = newTime.Minutes;

                if (timePicker.SelectedSecond != newTime.Seconds)
                    timePicker.SelectedSecond = newTime.Seconds;

                timePicker.EndUpdateSelectedTime();//signal that the selected time has been updated
                timePicker.OnTimeSelectedChanged(timePicker.SelectedTime, oldTime);
            }
        }

        private static void SetNewTime(TimePicker timePicker)
        {
            if (!timePicker.isUpdatingTime)
            {
                TimeSpan newTime = new TimeSpan(
                    timePicker.SelectedHour,
                    timePicker.SelectedMinute,
                    timePicker.SelectedSecond);
                //check if the time is the same
                if (timePicker.SelectedTime != newTime)
                    timePicker.SelectedTime = newTime;
            }
        }

        private static void TimePicker_HourChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TimePicker timePicker = (TimePicker)sender;

            //validate the hour set
            int hour = MathUtil.ValidateNumber(
                timePicker.SelectedHour,
                timePicker.hourMinValue,
                timePicker.hourMaxValue);
            if (hour != timePicker.SelectedHour)
                timePicker.SelectedHour = hour;

            //set the new timespan
            SetNewTime(timePicker);
        }

        private static void TimePicker_LimitsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TimePicker picker = (TimePicker)sender;
            picker.hourMinValue = picker.MinTime.Hours;
            picker.minuteMinValue = picker.MinTime.Minutes;
            picker.secondMinValue = picker.MinTime.Seconds;
            picker.CoerceValue(SelectedTimeProperty);//make sure to update the time if appropiate
        }

        private static void TimePicker_MinuteChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TimePicker timePicker = (TimePicker)sender;

            //validate the minute set
            int min = MathUtil.ValidateNumber(
                timePicker.SelectedMinute,
                timePicker.minuteMinValue,
                timePicker.minuteMaxValue);
            if (min != timePicker.SelectedMinute)
                timePicker.SelectedMinute = min;

            //set the new timespan
            SetNewTime(timePicker);
        }

        private static void TimePicker_SecondChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TimePicker timePicker = (TimePicker)sender;

            //validate the minute set
            int sec = MathUtil.ValidateNumber(timePicker.SelectedSecond,
                timePicker.secondMinValue, timePicker.secondMaxValue);
            if (sec != timePicker.SelectedSecond)
                timePicker.SelectedSecond = sec;

            //set the new timespan
            SetNewTime(timePicker);
        }

        private static void TrimSelectedText(TextBox textBox)
        {
            if (textBox.SelectionLength > 0)
                textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
        }

        private static void TryFocusNeighbourControl(TextBox currentControl, TextBox leftControl, TextBox rightControl, Key keyPressed)
        {
            if (keyPressed == Key.Left &&
                leftControl != null &&
                currentControl.CaretIndex == 0)
            {
                leftControl.Focus();
            }

            else if (keyPressed == Key.Right &&
                 rightControl != null &&
                 //if the caret index is the same as the length of the text and the user clicks right key it means that he wants to go to the next textbox
                 currentControl.CaretIndex == currentControl.Text.Length)
            {
                rightControl.Focus();
            }
        }

        private void BeginUpdateSelectedTime()
        {
            isUpdatingTime = true;
        }

        private void DecrementTime(object sender, RoutedEventArgs e)
        {
            IncrementDecrementTime(false);
        }

        private void EndUpdateSelectedTime()
        {
            isUpdatingTime = false;
        }

        private void HoursKeyUp(object sender, KeyEventArgs e)
        {
            //focus the next control
            TryFocusNeighbourControl(hours, null, minutes, e.Key);

            if (!IncrementDecrementTime(e.Key))
                ValidateAndSetHour(hours.Text);
        }

        private void IncrementDecrementTime(bool increment)
        {
            //check if hour is selected if yes set it
            if (hours == currentlySelectedTextBox)
                SelectedHour = MathUtil.IncrementDecrementNumber(hours.Text, hourMinValue, hourMaxValue, increment);

            //check if minute is selected if yes set it
            else if (minutes == currentlySelectedTextBox)
                SelectedMinute = MathUtil.IncrementDecrementNumber(minutes.Text, minuteMinValue, minuteMaxValue, increment);

            //if non of the above are selected assume that the seconds is selected
            else
                SelectedSecond = MathUtil.IncrementDecrementNumber(seconds.Text, secondMinValue, secondMaxValue, increment);
        }

        private bool IncrementDecrementTime(Key selectedKey)
        {
            if (selectedKey == Key.Up)
                IncrementDecrementTime(true);
            else if (selectedKey == Key.Down)
                IncrementDecrementTime(false);
            else
                return false;
            return true;
        }

        private void IncreaseTime(object sender, RoutedEventArgs e)
        {
            IncrementDecrementTime(true);
        }

        private void MinutesKeyUp(object sender, KeyEventArgs e)
        {
            //focus the next control
            TryFocusNeighbourControl(minutes, hours, seconds, e.Key);

            if (!IncrementDecrementTime(e.Key))
                ValidateAndSetMinute(minutes.Text);
        }

        private void OnTimeSelectedChanged(TimeSpan newTime, TimeSpan oldTime)
        {
            TimeSelectedChangedRoutedEventArgs args = new TimeSelectedChangedRoutedEventArgs(SelectedTimeChangedEvent)
            {
                NewTime = newTime,
                OldTime = oldTime
            };
            RaiseEvent(args);
        }

        private void SecondsKeyUp(object sender, KeyEventArgs e)
        {
            //focus the next control
            TryFocusNeighbourControl(seconds, minutes, null, e.Key);

            if (!IncrementDecrementTime(e.Key))
                ValidateAndSetSeconds(seconds.Text);
        }

        private void TextBox_HourTextChanged(object sender, TextCompositionEventArgs e)
        {
            //delete the text that is highlight(selected)
            TrimSelectedText(hours);

            //Adjust the text according to the carrot index
            string newText = AdjustText(hours, e.Text);

            //validates that the hour is correct if not set a valid value (0 or 24)
            int hourNum = ValidateAndSetHour(newText);

            //moves the carrot index or focus the neighbour
            AdjustCarretIndexOrMoveToNeighbour(hours, minutes);

            //handle the event so that it does not set the text, since we do it manually
            e.Handled = true;
        }

        private void TextBox_MinuteTextChanged(object sender, TextCompositionEventArgs e)
        {
            //delete the text that is highlight(selected)
            TrimSelectedText(minutes);

            //Adjust the text according to the carrot index
            string newText = AdjustText(minutes, e.Text);

            //validates that the minute is correct if not set a valid value (0 or 59)
            int minNum = ValidateAndSetMinute(newText);

            //moves the carrot index or focus the neighbour
            AdjustCarretIndexOrMoveToNeighbour(minutes, seconds);

            //handle the event so that it does not set the text, since we do it manually
            e.Handled = true;
        }

        private void TextBox_SecondTextChanged(object sender, TextCompositionEventArgs e)
        {
            //delete the text that is highlight(selected)
            TrimSelectedText(seconds);

            //Adjust the text according to the carrot index
            string newText = AdjustText(seconds, e.Text);

            //validates that the second is correct if not set a valid value (0 or 59)
            int secNum = ValidateAndSetSeconds(newText);

            //moves the carrot index or focus the neighbour
            AdjustCarretIndexOrMoveToNeighbour(seconds, null);

            //handle the event so that it does not set the text, since we do it manually
            e.Handled = true;
        }

        private void TextGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox selectedBox = (TextBox)sender;
            //set the currently selected textbox. 
            //This field is used to check which entity(hour/minute/second) to increment/decrement when user clicks the buttuns
            currentlySelectedTextBox = selectedBox;

            //highlight all code so that it is easier to the user to enter new info in the text box
            selectedBox.SelectAll();
        }

        //validates the hour passed as text and sets it to the SelectedHour property
        private int ValidateAndSetHour(string text)
        {
            int hourNum = MathUtil.ValidateNumber(text, hourMinValue, hourMaxValue);
            SelectedHour = hourNum;
            return hourNum;
        }

        //validates the minute passed as text and sets it to the SelectedMinute property
        private int ValidateAndSetMinute(string text)
        {
            int minNum = MathUtil.ValidateNumber(text, minuteMinValue, minuteMaxValue);
            SelectedMinute = minNum;
            return minNum;
        }

        //validates the second passed as text and sets it to the SelectedSecond property
        private int ValidateAndSetSeconds(string text)
        {
            int secNum = MathUtil.ValidateNumber(text, secondMinValue, secondMaxValue);
            SelectedSecond = secNum;
            return secNum;
        }
        #endregion

        #region Classes
        /// <summary>
        /// Routed event arguments for the TimeSelectedChanged event
        /// </summary>
        public class TimeSelectedChangedRoutedEventArgs : RoutedEventArgs
        {
            /// <summary>
            /// Gets or sets the new time
            /// </summary>
            public TimeSpan NewTime { get; set; }

            /// <summary>
            /// Gets or sets the old time
            /// </summary>
            public TimeSpan OldTime { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="routedEvent">The event that is raised </param>
            public TimeSelectedChangedRoutedEventArgs(RoutedEvent routedEvent)
                : base(routedEvent) { }
        }
        #endregion
    }
}

