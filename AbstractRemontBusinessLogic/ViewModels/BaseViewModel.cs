using AbstractRemontBusinessLogic.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// Базовый класс для View-моделей
    [DataContract]
    public abstract class BaseViewModel
    {
        [Column(visible: false)]
        [DataMember]
        public int Id { get; set; }
        public abstract List<string> Properties();
    }
}