using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Set up global exception handling
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            Exception ex = (Exception)args.ExceptionObject;
            MessageBox.Show($"Unhandled exception: {ex.Message}\n\nStack trace: {ex.StackTrace}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        // Initialize database context
        try
        {
            var context = DAL.DataContext.Context;
            // Force initialization of the database
            var thamSo = context.THAMSO.FirstOrDefault();
            var lops = context.LOP.ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error initializing database: {ex.Message}",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

