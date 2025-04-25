using System;

// Mediator interface for handling shipping quote requests
public interface IShippingQuoteMediator
{
    void ProcessRequest(ShippingQuoteRequest request);
}

// Request class to hold package information
public class ShippingQuoteRequest
{
    public double Weight { get; private set; }
    public double Width { get; private set; }
    public double Height { get; private set; }
    public double Length { get; private set; }
    public bool IsValid { get; set; } = true;

    // Fluent interface methods for setting package dimensions
    public ShippingQuoteRequest WithWeight(double weight)
    {
        Weight = weight;
        return this;
    }

    public ShippingQuoteRequest WithWidth(double width)
    {
        Width = width;
        return this;
    }

    public ShippingQuoteRequest WithHeight(double height)
    {
        Height = height;
        return this;
    }

    public ShippingQuoteRequest WithLength(double length)
    {
        Length = length;
        return this;
    }
}

// Concrete mediator implementing the shipping quote logic
public class ShippingQuoteMediator : IShippingQuoteMediator
{
    private const double MaxWeight = 50;
    private const double MaxDimensions = 50;

    public void ProcessRequest(ShippingQuoteRequest request)
    {
        // Validate weight
        if (request.Weight > MaxWeight)
        {
            Console.WriteLine("Package too heavy to be shipped via Package Express. Have a good day.");
            request.IsValid = false;
            return;
        }

        // Validate dimensions
        double totalDimensions = request.Width + request.Height + request.Length;
        if (totalDimensions > MaxDimensions)
        {
            Console.WriteLine("Package too big to be shipped via Package Express.");
            request.IsValid = false;
            return;
        }

        // Calculate and display quote
        if (request.IsValid)
        {
            double quote = CalculateQuote(request);
            DisplayQuote(quote);
        }
    }

    private double CalculateQuote(ShippingQuoteRequest request)
    {
        return (request.Width * request.Height * request.Length * request.Weight) / 100;
    }

    private void DisplayQuote(double quote)
    {
        Console.WriteLine($"Your estimated total for shipping this package is: ${quote:F2}");
        Console.WriteLine("Thank you!");
    }
}

// Input handler class for getting and validating user input
public class InputHandler
{
    public static double GetValidInput(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            if (double.TryParse(Console.ReadLine(), out double value) && value > 0)
            {
                return value;
            }
            Console.WriteLine("Please enter a valid positive number.");
        }
    }
}

// Main program class
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Display welcome message
            Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");

            // Create mediator and request objects
            IShippingQuoteMediator mediator = new ShippingQuoteMediator();
            var request = new ShippingQuoteRequest()
                .WithWeight(InputHandler.GetValidInput("Please enter the package weight:"))
                .WithWidth(InputHandler.GetValidInput("Please enter the package width:"))
                .WithHeight(InputHandler.GetValidInput("Please enter the package height:"))
                .WithLength(InputHandler.GetValidInput("Please enter the package length:"));

            // Process the shipping quote request
            mediator.ProcessRequest(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}