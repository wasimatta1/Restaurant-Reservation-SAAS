using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using System.Collections;

namespace RestaurantReservation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Select an action:");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Update");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. Get By ID");
                Console.WriteLine("5. Get All");
                Console.WriteLine("0. Exit");
                var choice = Console.ReadLine();
                Console.WriteLine();

                if (string.IsNullOrEmpty(choice) || !"012345".Contains(choice))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }
                if (choice == "0")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                Console.WriteLine("Select an entity:");
                Console.WriteLine("1. Customer");
                Console.WriteLine("2. Employee");
                Console.WriteLine("3. Order");
                Console.WriteLine("4. MenuItem");
                Console.WriteLine("5. Restaurant");
                Console.WriteLine("6. Reservation");
                Console.WriteLine("7. Table");

                var entityChoice = Console.ReadLine();
                Console.WriteLine();


                if (string.IsNullOrEmpty(entityChoice) || !"1234567".Contains(entityChoice))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                string objectName = entityChoice switch
                {
                    "1" => nameof(Customer),
                    "2" => nameof(Employee),
                    "3" => nameof(Order),
                    "4" => nameof(MenuItem),
                    "5" => nameof(Restaurant),
                    "6" => nameof(Reservation),
                    "7" => nameof(Table)
                };

                var type = Type.GetType($"RestaurantReservation.Db.Entities.{objectName}" +
                    $",RestaurantReservation.Db");

                var instance = Activator.CreateInstance(type!);

                int id = 0;

                if ("12".Contains(choice))
                    instance = ReadInput(instance!);
                else if ("34".Contains(choice))
                    id = ReadID();


                var repositoryType = Type.GetType($"RestaurantReservation.Db.Repositories.Implementations.{objectName}Repository" +
                $",RestaurantReservation.Db");

                Console.WriteLine("Please wait...");

                var context = new RestaurantReservationDbContext();

                var repositoryInstance = Activator.CreateInstance(repositoryType!, context);



                switch (choice)
                {
                    case "1":
                        await (Task)repositoryInstance.GetType().GetMethod("AddAsync").Invoke(repositoryInstance, new object[] { instance });
                        Console.WriteLine("Entity added successfully.");
                        break;

                    case "2":
                        await (Task)repositoryInstance.GetType().GetMethod("UpdateAsync").Invoke(repositoryInstance, new object[] { instance });
                        Console.WriteLine("Entity updated successfully.");
                        break;

                    case "3":
                        await (Task)repositoryInstance.GetType().GetMethod("DeleteAsync").Invoke(repositoryInstance, new object[] { id });
                        Console.WriteLine("Entity deleted successfully.");
                        break;

                    case "4":
                        var getByIdTask = (Task)repositoryInstance.GetType().GetMethod("GetByIdAsync").Invoke(repositoryInstance, new object[] { id });
                        await getByIdTask.ConfigureAwait(false);

                        var getByIdResultProperty = getByIdTask.GetType().GetProperty("Result");
                        var getByIdResult = getByIdResultProperty?.GetValue(getByIdTask);

                        Console.WriteLine(getByIdResult ?? "No entity found with the given ID.");
                        break;

                    case "5":
                        var getAllTask = (Task)repositoryInstance.GetType().GetMethod("GetAllAsync").Invoke(repositoryInstance, null);
                        await getAllTask.ConfigureAwait(false);

                        var getAllResultProperty = getAllTask.GetType().GetProperty("Result");
                        var getAllResult = getAllResultProperty?.GetValue(getAllTask);

                        if (getAllResult is IEnumerable enumerable)
                        {
                            foreach (var item in enumerable)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No entities found.");
                        }
                        break;
                }

            }
        }

        public static object ReadInput(object instance)
        {
            var properties = instance.GetType().GetProperties();

            foreach (var property in properties)
            {

                bool isClass = property.PropertyType.IsClass
                    && property.PropertyType != typeof(string);

                bool isCollection = typeof(System.Collections.IEnumerable).
                    IsAssignableFrom(property.PropertyType)
                    && property.PropertyType != typeof(string);

                if (isClass || isCollection)
                {
                    continue;
                }

                dynamic value;
                do
                {
                    Console.WriteLine($"Enter {property.Name}:");
                    value = Console.ReadLine();

                    if (string.IsNullOrEmpty(value))
                    {
                        Console.WriteLine("Input cannot be empty. Please try again.");
                    }
                } while (string.IsNullOrEmpty(value));

                value = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(instance, value);
            }

            return instance;
        }
        public static int ReadID()
        {
            int id;
            do
            {
                Console.WriteLine("Enter ID:");
            } while (!int.TryParse(Console.ReadLine(), out id));

            return id;
        }

    }
}
