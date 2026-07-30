namespace XYZ_shop.Application.Dtos
{
    public class EditGameFormDto
    {
        public EditGameDto Game { get; set; } = new();
        public GameFormOptionsDto Options { get; set; } = new();
    }
}
