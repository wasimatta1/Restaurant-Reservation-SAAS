
namespace RestaurantReservation.Extenstions
{
    public static class ExtensionMethod
    {
        public static async Task Print<T>(this Task<IEnumerable<T>> source, string title)
        {
            if (source == null)
                return;
            Console.WriteLine();
            Console.WriteLine("┌────────────────────────────────────┐");
            Console.WriteLine($"│   {title.PadRight(52, ' ')}│");
            Console.WriteLine("└────────────────────────────────────┘");
            Console.WriteLine();
            foreach (var item in source.Result)
            {
                Console.WriteLine(item);

            }
        }
    }
}
