using Demo.Services;
using Microsoft.Extensions.Logging;

namespace Demo.NETWinForm
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> logger;
        private readonly IOrderSvc orderSvc;
        public MainForm()
        {
            InitializeComponent();
        }


        public MainForm(IOrderSvc orderSvc, ILogger<MainForm> logger)
        {
            InitializeComponent();

            this.orderSvc = orderSvc;
            this.logger = logger;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            var orders = orderSvc.GetAll(null);

            var order = orderSvc.GetSingle(orders.ToList()[0].Id);

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            logger.LogInformation("Show Form2");

            var orders = orderSvc.GetAll(null);
            var order = orderSvc.GetSingle(orders.ToList()[0].Id);
        }


    }
}