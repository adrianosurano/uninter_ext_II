using System.ComponentModel;

namespace fijate.ui.maui
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        Dictionary<int, IDispatcherTimer> Lembretes = [];

        public MainPage() => InitializeComponent();

        private async Task Mensagem(int idx, IDispatcherTimer timer)
        {
            if (Lembretes.Count <= 0)
                return;

            var tit = "";
            var msg = "";
            switch (idx)
            {
                case 0:
                    tit = "Lembrete de descanso breve";
                    msg = "É hora de fazer uma pausa!";
                    break;
                case 1:
                    tit = "Lembrete de exercício ocular";
                    msg = "Que tal praticar um dos exercícios oculares?";
                    break;
                case 2:
                    tit = "Lembrete de hidratação";
                    msg = "É hora de se hidratar!";
                    break;
            }

            if (string.IsNullOrWhiteSpace(tit) || string.IsNullOrWhiteSpace(msg))
                return;

            timer.Stop();
            await DisplayAlert(tit, msg, "OK");
            timer.Start();
        }

        private async void T0_Tick(object? sender, EventArgs e) => await Mensagem(0, (IDispatcherTimer)sender!);
        private async void T1_Tick(object? sender, EventArgs e) => await Mensagem(1, (IDispatcherTimer)sender!);
        private async void T2_Tick(object? sender, EventArgs e) => await Mensagem(2, (IDispatcherTimer)sender!);

        private void CriaLembrete(int idx, Picker opcao)
        {
            var sOpcao = opcao.SelectedItem.ToString()!.Replace("A cada", "").Replace("(recomendado)", "").Replace("minutos", "").Replace("hora", "").Trim();
            sOpcao = string.IsNullOrWhiteSpace(sOpcao) ? "60" : sOpcao;

            var iOpcao = int.Parse(sOpcao);

            var tmr = Dispatcher.CreateTimer();
            tmr.Interval = TimeSpan.FromMinutes(iOpcao);

            switch (idx)
            {
                case 0:
                    tmr.Tick += T0_Tick;
                    break;
                case 1:
                    tmr.Tick += T1_Tick;
                    break;
                case 2:
                    tmr.Tick += T2_Tick;
                    break;
            }

            Lembretes.Add(idx, tmr);
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            Lembretes.Clear();

            CriaLembrete(0, T0);
            CriaLembrete(1, T1);
            CriaLembrete(2, T2);

            foreach (var item in Lembretes)
                item.Value.Start();

            await DisplayAlert("Fijate", "Lembretes configurados com sucesso!", "OK");
        }
    }

}
