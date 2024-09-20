using System;
using System.Collections.Generic;

namespace FelixHelper.Models;
public class Consts
{
    public static Dictionary<string, string> Months = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            { "январь", "январе"},
            { "феварль", "феврале"},
            { "март", "марте"},
            { "апрель", "апреле"},
            { "май", "мае"},
            { "июнь", "июне"},
            { "июль", "июле"},
            { "август", "августе"},
            { "сентябрь", "сентябре"},
            { "октябрь", "октябре"},
            { "ноябрь", "ноябре"},
            { "декабрь", "декабре"}
        };

    public const string UnkownCommand = "Нет такой команды";
    public const string UnkownFormat = "Неправильный формат данных";
    public const string UnkownMonth = "Неизвестный месяц";
    public const string PromotionsUpdated = "Даты изменены";
}
