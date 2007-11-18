using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NUnit.Framework;

namespace NHibernate.Test.DialectTest
{
	[TestFixture]
	public class MsSql2005DialectFixture
	{
		[Test]
		public void GetLimitString()
		{
			MsSql2005Dialect d = new MsSql2005Dialect();

			SqlString str = d.GetLimitString(new SqlString("SELECT fish.id FROM fish"), 0, 10);
			Assert.AreEqual(
				"SELECT TOP 10 id FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__) as row, query.id, query.__hibernate_sort_expr_1__ FROM (SELECT fish.id, CURRENT_TIMESTAMP as __hibernate_sort_expr_1__ FROM fish) query ) page WHERE page.row > 0 ORDER BY __hibernate_sort_expr_1__",
				str.ToString());

			str = d.GetLimitString(new SqlString("SELECT DISTINCT fish_.id FROM fish fish_"), 0, 10);
			Assert.AreEqual(
				"SELECT TOP 10 id FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__) as row, query.id, query.__hibernate_sort_expr_1__ FROM (SELECT DISTINCT fish_.id, CURRENT_TIMESTAMP as __hibernate_sort_expr_1__ FROM fish fish_) query ) page WHERE page.row > 0 ORDER BY __hibernate_sort_expr_1__",
				str.ToString());

			str = d.GetLimitString(new SqlString("SELECT DISTINCT fish_.id as ixx9_ FROM fish fish_"), 0, 10);
			Assert.AreEqual(
				"SELECT TOP 10 ixx9_ FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__) as row, query.ixx9_, query.__hibernate_sort_expr_1__ FROM (SELECT DISTINCT fish_.id as ixx9_, CURRENT_TIMESTAMP as __hibernate_sort_expr_1__ FROM fish fish_) query ) page WHERE page.row > 0 ORDER BY __hibernate_sort_expr_1__",
				str.ToString());

			str = d.GetLimitString(new SqlString("SELECT * FROM fish ORDER BY name"), 5, 15);
			Assert.AreEqual(
				"SELECT TOP 15 * FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__) as row, query.*, query.__hibernate_sort_expr_1__ FROM (SELECT *, name as __hibernate_sort_expr_1__ FROM fish) query ) page WHERE page.row > 5 ORDER BY __hibernate_sort_expr_1__",
				str.ToString());

			str = d.GetLimitString(new SqlString("SELECT fish.id, fish.name FROM fish ORDER BY name DESC"), 7, 28);
			Assert.AreEqual(
				"SELECT TOP 28 id, name FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__ DESC) as row, query.id, query.name, query.__hibernate_sort_expr_1__ FROM (SELECT fish.id, fish.name, name as __hibernate_sort_expr_1__ FROM fish) query ) page WHERE page.row > 7 ORDER BY __hibernate_sort_expr_1__ DESC",
				str.ToString());

			str =
				d.GetLimitString(
					new SqlString("SELECT * FROM fish LEFT JOIN (SELECT * FROM meat ORDER BY weight) AS t ORDER BY name DESC"), 10, 20);
			Assert.AreEqual(
				"SELECT TOP 20 * FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__ DESC) as row, query.*, query.__hibernate_sort_expr_1__ FROM (SELECT *, name as __hibernate_sort_expr_1__ FROM fish LEFT JOIN (SELECT * FROM meat ORDER BY weight) AS t) query ) page WHERE page.row > 10 ORDER BY __hibernate_sort_expr_1__ DESC",
				str.ToString());

			str = d.GetLimitString(new SqlString("SELECT *, (SELECT COUNT(1) FROM fowl WHERE fish_id = fish.id) AS some_count FROM fish"), 0, 10);
			Assert.AreEqual(
				"SELECT TOP 10 *, some_count FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__) as row, query.*, query.some_count, query.__hibernate_sort_expr_1__ FROM (SELECT *, (SELECT COUNT(1) FROM fowl WHERE fish_id = fish.id) AS some_count, CURRENT_TIMESTAMP as __hibernate_sort_expr_1__ FROM fish) query ) page WHERE page.row > 0 ORDER BY __hibernate_sort_expr_1__",
				str.ToString());

			str = d.GetLimitString(new SqlString("SELECT * FROM fish WHERE scales = ", Parameter.Placeholder), 0, 10);
			Assert.AreEqual(
				"SELECT TOP 10 * FROM (SELECT ROW_NUMBER() OVER(ORDER BY __hibernate_sort_expr_1__) as row, query.*, query.__hibernate_sort_expr_1__ FROM (SELECT *, CURRENT_TIMESTAMP as __hibernate_sort_expr_1__ FROM fish WHERE scales = ?) query ) page WHERE page.row > 0 ORDER BY __hibernate_sort_expr_1__",
				str.ToString());
		}
	}
}
