namespace ClassLibraryOfUtils;

public static class FileHeader
{
    /// <summary>
    /// Static string array stores column headers in English.
    /// </summary>
    public static string[] HeadInEng { get; } = {
        "ID",
        "global_id",
        "Name",
        "AdmArea",
        "District",
        "ParkName",
        "WiFiName",
        "CoverageArea",
        "FunctionFlag",
        "AccessFlag",
        "Password",
        "Longitude_WGS84",
        "Latitude_WGS84",
        "geodata_center",
        "geoarea"
    };

    /// <summary>
    /// Static string array stores column headers in Russian.
    /// </summary>
    public static string[] HeadInRus { get; } ={
        "Код",
        "global_id",
        "Наименование",
        "Административный округ по адресу",
        "Район",
        "Наименование парка",
        "Имя Wi-Fi сети",
        "Зона покрытия (метры)",
        "Признак функционирования",
        "Условия доступа",
        "Пароль",
        "Долгота в WGS-84",
        "Широта в WGS-84",
        "geodata_center",
        "geoarea"
    };

}