namespace Tgs.VipLevels.Cycle.Core.UnitTest;

public class DateTimeUtensilUnitTest
{
	[Fact]
	public void GetTimeZone_2024_03_04_16_00_01_00()
	{
		var source = new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var actual = sut.GetTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 03, 05, 00, 00, 01, TimeSpan.FromHours(8)), actual);
	}

	[Fact]
	public void GetTimeZone_2024_03_04_16_00_01_08()
	{
		var source = new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.FromHours(8));

		var sut = new DateTimeUtensil();

		var actual = sut.GetTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.FromHours(8)), actual);
	}

	[Fact]
	public void GetDayTimeZone_2024_03_04_16_00_01_00()
	{
		var source = new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var actual = sut.GetDayTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 03, 05, 00, 00, 00, TimeSpan.FromHours(8)), actual);
	}

	[Fact]
	public void GetDayTimeZone_2024_03_04_16_00_01_08()
	{
		var source = new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.FromHours(8));

		var sut = new DateTimeUtensil();

		var actual = sut.GetDayTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 03, 04, 00, 00, 00, TimeSpan.FromHours(8)), actual);
	}

	[Fact]
	public void GetMonthsTimeZone_2024_03_04_16_00_01_00()
	{
		var source = new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetMonthsTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 03, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 04, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}

	[Fact]
	public void GetMonthsTimeZone_2024_02_29_15_59_59_00()
	{
		var source = new DateTimeOffset(2024, 02, 29, 15, 59, 59, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetMonthsTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 02, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 03, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}

	[Fact]
	public void GetSeasonTimeZone_2023_12_31_15_59_59_00()
	{
		var source = new DateTimeOffset(2023, 12, 31, 15, 59, 59, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetSeasonTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}

	[Fact]
	public void GetSeasonTimeZone_2023_12_31_16_00_01_00()
	{
		var source = new DateTimeOffset(2023, 12, 31, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetSeasonTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 04, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}

	[Fact]
	public void GetSeasonTimeZone_2024_01_31_16_00_01_00()
	{
		var source = new DateTimeOffset(2024, 01, 31, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetSeasonTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 04, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}

	[Fact]
	public void GetSeasonTimeZone_2024_02_29_16_00_01_00()
	{
		var source = new DateTimeOffset(2024, 02, 29, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetSeasonTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 04, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}

	[Fact]
	public void GetSeasonTimeZone_2024_03_04_16_00_01_00()
	{
		var source = new DateTimeOffset(2024, 03, 04, 16, 00, 01, TimeSpan.Zero);

		var sut = new DateTimeUtensil();

		var (FromOn, ToOn) = sut.GetSeasonTimeZone(source, (int)TimeSpan.FromHours(+8).TotalMinutes);

		Assert.Equal(new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.FromHours(8)), FromOn);
		Assert.Equal(new DateTimeOffset(2024, 04, 01, 00, 00, 00, TimeSpan.FromHours(8)), ToOn);
	}
}
