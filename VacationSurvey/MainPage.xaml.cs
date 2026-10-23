using System;
using Microsoft.Maui.Controls;

namespace VacationSurvey;

public partial class MainPage : ContentPage
{
    // Запоминаем значения по умолчанию, чтобы вернуть анкету в исходное состояние
    private readonly DateTime _defaultDate = DateTime.Now;
    private const double DefaultDaysValue = 7;

    public MainPage()
    {
        InitializeComponent();

        // Диапазон дат: от сегодня до +365 дней, по умолчанию - через месяц (внутри диапазона)
        TripDatePicker.MinimumDate = _defaultDate;
        TripDatePicker.MaximumDate = _defaultDate.AddYears(1);
        TripDatePicker.Date = _defaultDate.AddDays(30);
    }

    // Включаем кнопку "Показать результат" только когда заполнены оба обязательных поля
    private void OnRequiredFieldChanged(object sender, TextChangedEventArgs e)
    {
        bool nameFilled = !string.IsNullOrWhiteSpace(NameEntry.Text);
        bool emailFilled = !string.IsNullOrWhiteSpace(EmailEntry.Text);

        ShowResultButton.IsEnabled = nameFilled && emailFilled;
    }

    // Просто триггерим обновление привязанного лейбла (сама привязка через x:Reference уже это делает)
    private void OnDaysStepperChanged(object sender, ValueChangedEventArgs e)
    {
        // Ничего дополнительно делать не нужно - DaysLabel обновляется через x:Reference Binding
    }

    private void OnShowResultClicked(object sender, EventArgs e)
    {
        // Проверяем минимум два обязательных поля
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            ResultLabel.Text = "Пожалуйста, заполните имя и email.";
            ResultLabel.IsVisible = true;
            return;
        }

        string destination = DestinationPicker.SelectedItem?.ToString() ?? "не выбрано";
        string budget = BudgetEconomy.IsChecked ? "Эконом"
                        : BudgetStandard.IsChecked ? "Стандарт"
                        : "Люкс";

        var interests = new System.Collections.Generic.List<string>();
        if (SightseeingCheck.IsChecked) interests.Add("Экскурсии");
        if (BeachCheck.IsChecked) interests.Add("Пляжный отдых");
        string interestsText = interests.Count > 0 ? string.Join(", ", interests) : "не выбраны";

        string summary =
            $"Имя: {NameEntry.Text}\n" +
            $"Email: {EmailEntry.Text}\n" +
            $"Комментарий: {(string.IsNullOrWhiteSpace(CommentsEditor.Text) ? "—" : CommentsEditor.Text)}\n" +
            $"Направление: {destination}\n" +
            $"Бюджет: {budget}\n" +
            $"Интересы: {interestsText}\n" +
            $"Рассылка: {(NewsletterSwitch.IsToggled ? "да" : "нет")}\n" +
            $"Дата поездки: {TripDatePicker.Date:dd.MM.yyyy}\n" +
            $"Количество дней: {DaysStepper.Value:F0}";

        ResultLabel.Text = summary;
        ResultLabel.IsVisible = true;
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        NameEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        CommentsEditor.Text = string.Empty;
        DestinationPicker.SelectedItem = null;

        BudgetEconomy.IsChecked = true;
        SightseeingCheck.IsChecked = false;
        BeachCheck.IsChecked = false;
        NewsletterSwitch.IsToggled = false;

        TripDatePicker.Date = _defaultDate;
        DaysStepper.Value = DefaultDaysValue;

        ResultLabel.Text = string.Empty;
        ResultLabel.IsVisible = false;

        ShowResultButton.IsEnabled = false;
    }
}
