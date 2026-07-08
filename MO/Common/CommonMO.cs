using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Common
{
    public class result
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string? id { get; set; }
        public dynamic data { get; set; }
        public string redirectUrl { get; set; }
        public bool isRedirect { get; set; }
        public bool isPost { get; set; }      
        public string source { get; set; } 
    }

    public class ListData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public dynamic data { get; set; }
    }
    public class CommonFilter
    {
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? status { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option2 { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option3 { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option4 { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option5 { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option6 { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option7 { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid characters in input.")]
        public string? option8 { get; set; }

        public string? groupId { get; set; }
        public string? url { get; set; }
    }
    public class targetgraphinput
    {
        public string session { get; set; }
        public string indicator { get; set; }
        public string clickedLabel { get; set; }
        public string clickedValue { get; set; }
        public string datasetLabel { get; set; }

    }
 
    public class resultTargetGraph
    {
        public bool status { get; set; }
        public List<datapoint> Currentgarph { get; set; }
        public List<datapoint> AchiveGraph { get; set; }
        public List<datapoint> TargetGraph { get; set; }
        public string indicatorname { get; set; }
        public string unit { get; set; }
        public string redirectUrl { get; set; }
        public bool isRedirect { get; set; }
        public bool isPost { get; set; }
    }

    public class datapoint
    {
        public string year { get; set; }
        public decimal? datavakue { get; set; }
    }

    public class input_datafiled
    {
        [Range(2, int.MaxValue, ErrorMessage = "Target value must be a non-negative number.")]
        public int Id { get; set; }
        [Range(2, int.MaxValue, ErrorMessage = "Target value must be a non-negative number.")]
        public int Id2 { get; set; }
        [Range(2, int.MaxValue, ErrorMessage = "Target value must be a non-negative number.")]
        public int Id3 { get; set; }
        public int Id4 { get; set; }
    }

    public class IndicatorModel
    {
        public string Basevalue { get; set; }
        public string TargetValue { get; set; }
        // Add other fields if needed
    }

    public class FinancialYearDDL
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
