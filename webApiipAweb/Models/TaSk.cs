using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TaskWithOpenAnsw : ParentTask
    {
        public TaskWithOpenAnsw()
        {
            TypesTask = TypesTask.Opened;
        }

        public double fine { get; set; }

        public bool orderImportant { get; set; }
        
        public ResultTypes ResultType { get; set; }

        public string GetResultType() => this.ResultType switch
        {
            ResultTypes.Label =>"Текстовое поле",
            ResultTypes.Squares => "Квадратики",
            ResultTypes.Table => "Таблица",
            _ =>"Что-то не так, значение не было присвоено"
        };

        public string htmlModel { get; set; }

        public virtual List<AnswearOnTaskOpen> AnswearOnTaskOpens { get; set; } = new List<AnswearOnTaskOpen>();

    }

    public class TaskWithClosedAnsw : ParentTask
    {
        public TaskWithClosedAnsw()
        {
            TypesTask = TypesTask.Closed;
        }
        public virtual List<AnswearOnTask> AnswearOnTask { get; set; } = new List<AnswearOnTask>();
    }

    public enum ResultTypes
    {
        Label,
        Squares,
        Table
    }
}
