using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HallOfFame.Models
{
    public class Skill
    {
        /// <summary>
        /// Идентификатор навыка
        /// </summary>
        public long SkillId { get; set; } 
        /// <summary>
        /// Название навыка
        /// </summary>
        [Required(ErrorMessage = "Укажите название навыка")]
        public string Name { get; set; }

        /// <summary>
        /// Уровень владения навыком
        /// </summary>
        [Required(ErrorMessage = "Укажите уровень навыка")]
        [Range(1, 10, ErrorMessage = "Уровень должен быть в промежутке от 1 до 10")]
        public byte Level { get; set; } // 1-10
        /// <summary>
        /// Свойство обратной навигации
        /// </summary>
        [JsonIgnore]
        public Person Person { get; set; }
    }
}
