using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HallOfFame.Models
{
    public class Person
    {
        public Person()
        {
            Skills = new List<Skill>();
        }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Укажите имя пользователя")]
        public string Name { get; set; }

        /// <summary>
        /// Отображаемое имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Укажите отображаемое имя пользователя")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Набор навыков пользователя
        /// </summary>
        public List<Skill> Skills { get; set; }
    }
}
