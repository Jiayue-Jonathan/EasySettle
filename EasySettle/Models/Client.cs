using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySettle.Models;

public class Client 
{

    
    public int ClientID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    [RegularExpression(@"^\d{9}$")] // Regular expression pattern for exactly 9 digits
    public int telNo { get; set; }

}


