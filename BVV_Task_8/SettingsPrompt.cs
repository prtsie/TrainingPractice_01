namespace BVV_Task_8;

public sealed partial class SettingsPrompt : Form
{
    public Settings Result { get; private set; } = null!;
    
    public SettingsPrompt()
    {
        InitializeComponent();
        MinimumSize = MaximumSize = Size;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        sizeInput.Minimum = 0;
        sizeInput.Value -= sizeInput.Value % 2;
        sizeInput.Value++;
        Result = new()
        {
            Size = (int)sizeInput.Value,
            TimeInterval = (int)intervalInput.Value,
            ImmunityIgnoreChance = immunityIgnoreChanceInput.Value / 100m
        };
        DialogResult = DialogResult.OK;
    }
}