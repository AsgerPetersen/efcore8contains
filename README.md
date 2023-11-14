Demonstrates what looks like a bug in efcore8 optimization of `Contains` for SQL server.

To reproduce:
```bash
# Up a SQLserver
docker compose up db

# This uses .NET7 and EfCore7 and it works
dotnet run --project efcore8contains.net7.csproj 

# This uses .NET8 and EfCore8 and it throws an exception
dotnet run --project efcore8contains.net8.csproj 
```

This exception is thrown by .NET8/EfCore8:
```
Unhandled exception. System.InvalidOperationException: The LINQ expression '__types_0
    .Contains(MaterializeCollectionNavigation(
        Navigation: Person.Children,
        subquery: DbSet<Child>()
            .Where(i => EF.Property<int?>(StructuralTypeShaperExpression: 
                ContainsOptimization.Person
                ValueBufferExpression: 
                    ProjectionBindingExpression: EmptyProjectionMember
                IsNullable: False
            , "Id") != null && object.Equals(
                objA: (object)EF.Property<int?>(StructuralTypeShaperExpression: 
                    ContainsOptimization.Person
                    ValueBufferExpression: 
                        ProjectionBindingExpression: EmptyProjectionMember
                    IsNullable: False
                , "Id"), 
                objB: (object)EF.Property<int?>(i, "PersonId"))))
        .AsQueryable()
        .Any() ? Happy : Sad)' could not be translated. Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to 'AsEnumerable', 'AsAsyncEnumerable', 'ToList', or 'ToListAsync'. See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.
   at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.Translate(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.Translate(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.TranslateSubquery(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
   at Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerSqlTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
   at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.TranslateInternal(Expression expression, Boolean applyDefaultTypeMapping)
   at Microsoft.EntityFrameworkCore.Query.RelationalSqlTranslatingExpressionVisitor.Translate(Expression expression, Boolean applyDefaultTypeMapping)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.TranslateExpression(Expression expression, Boolean applyDefaultTypeMapping)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.TranslateLambdaExpression(ShapedQueryExpression shapedQueryExpression, LambdaExpression lambdaExpression)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.TranslateWhere(ShapedQueryExpression source, LambdaExpression predicate)
   at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
   at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.Translate(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.Translate(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.QueryCompilationContext.CreateQueryExecutor[TResult](Expression query)
   at Microsoft.EntityFrameworkCore.Storage.Database.CompileQuery[TResult](Expression query, Boolean async)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.CompileQueryCore[TResult](IDatabase database, Expression query, IModel model, Boolean async)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.<>c__DisplayClass9_0`1.<Execute>b__0()
   at Microsoft.EntityFrameworkCore.Query.Internal.CompiledQueryCache.GetOrAddQuery[TResult](Object cacheKey, Func`1 compiler)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.Execute[TResult](Expression query)
   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.Execute[TResult](Expression expression)
   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetEnumerator()
   at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at ContainsOptimization.Program.Main(String[] args) in /Users/asger/Code/Aika/Aika-DevHelpers/efcore8contains/Program.cs:line 19
```