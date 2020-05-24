using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.BindingModels
{
    //Клиент
   public class ClientBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string FIO { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}
