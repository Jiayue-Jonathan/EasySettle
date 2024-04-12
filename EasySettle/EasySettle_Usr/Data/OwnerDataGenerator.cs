using System;
using System.Collections.Generic;
using Bogus;
using EasySettle.Models;

namespace EasySettle.Utilities
{
public class OwnerDataGenerator
{
    public static List<Owner> GenerateOwners(int numberOfOwners)
    {
        var ownerId = 20000; // Starting ID
        var telNoGenerator = new Random();
        
        var ownerFaker = new Faker<Owner>()
            .RuleFor(o => o.OwnerID, f => ownerId++) // Incremental IDs
            .RuleFor(o => o.FirstName, f => f.Name.FirstName())
            .RuleFor(o => o.LastName, f => f.Name.LastName())
            .RuleFor(o => o.Email, (f, o) => f.Internet.Email(o.FirstName, o.LastName))
            .RuleFor(o => o.telNo, f => Convert.ToInt32(f.Phone.PhoneNumber("#########"))); // Generates a 9-digit number
        
        return ownerFaker.Generate(numberOfOwners); // Generate the specified number of owners
    }
}
}




