# IfElseBracketDiagnostic

1. Opprett et nytt C#-prosjekt i VS2015. Bruk templaten "Diagnostic with Code Fix" som ligger under Extensibility. Kall prosjektet "IfElseBracketDiagnostic".

   Prosjektet du n� har opprettet er et eksempelprosjekt som gir en warning hvis en property er skrevet med sm� bokstaver. Prosjektet inneholder ogs� en fiks som endrer propertien til � ha store bokstaver.  

   Vi skal skrive om prosjektet til � gi en warning hvis man ikke innkapsulerer if- og else-statements i curly brackets, og en fiks som gj�r dette for deg.

2. �pne DiagnosticAnalyzer.cs. Endre strengene Title, MessageFormat og Category til noe mer passende, f.eks. henholdsvis "If and else statements must use braces", "'{0}' statements must have braces" og "Formatting".

3. Fjern innholdet i Initialize-metoden og erstatt det med f�lgende linje:

    context.RegisterSyntaxNodeAction(AnalyzeIfElseStatement, SyntaxKind.IfStatement, SyntaxKind.ElseClause);

4. Fjern AnalyzeSymbol-metoden og lag f�lgende metode i stedet:

    private static void AnalyzeIfElseStatement(SyntaxNodeAnalysisContext context)
    {
       // TODO: Implement
    }

5. I metoden sjekker vi om noden er en if-statement, og om den i s� fall ikke er en blokk, alts� mangler curly brackets. Hvis dette er tilfellet kaller vi ReportDiagnostic.

    var ifStatement = context.Node as IfStatementSyntax;
    
    if (ifStatement != null 
        && ifStatement.Statement != null 
        && ifStatement.Statement.IsKind(SyntaxKind.Block) == false)
    {
        var diagnostic = Diagnostic.Create(Rule, ifStatement.GetLocation(), "if");
        context.ReportDiagnostic(diagnostic);
    }

6. I samme metode gj�r vi tilsvarende med else

    var elseSyntax = context.Node as ElseClauseSyntax;
    
    if (elseSyntax != null
        && elseSyntax.Statement != null
        && elseSyntax.Statement.IsKind(SyntaxKind.IfStatement) == false
        && elseSyntax.Statement.IsKind(SyntaxKind.Block) == false)
    {
        var diagnostic = Diagnostic.Create(Rule, elseSyntax.GetLocation(), "else");
        context.ReportDiagnostic(diagnostic);
    }

7. Klikk "Start" eller F5. Dette vil starte en ny instans av Visual Studio. Opprett en ny Console Application og skriv en if-statement uten curly brackets i Main-metoden. Dette skal gi en warning.

8. Avslutt den nye instansen av Visual Studio og �pne CodeFixProvider.cs.

9. Bytt ut innholdet i ComputeFixesAsync med f�lgende kode:

    var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
    var diagnostic = context.Diagnostics.First();
    var diagnosticSpan = diagnostic.Location.SourceSpan;
    
    // Find the type declaration identified by the diagnostic.
    var errorToken = root.FindToken(diagnosticSpan.Start).Parent;
    
    // Register a code action that will invoke the fix.
    context.RegisterFix(
        CodeAction.Create("Add brackets", c => AddBracesAsync(context.Document, errorToken, c)),
        diagnostic);

10. Slett MakeUppercaseAsync og opprett f�lgende metode i stedet:

    private async Task<Document> AddBracketsAsync(Document document, SyntaxNode errorStatement, CancellationToken cancellationToken)
    {
        SyntaxNode toReplace = null;
    
        if (errorStatement is IfStatementSyntax)
        {
            var newIfStatement = errorStatement as IfStatementSyntax;
            newIfStatement = newIfStatement.WithStatement(SyntaxFactory.Block(newIfStatement.Statement)).WithAdditionalAnnotations(Formatter.Annotation);
            toReplace = newIfStatement;
        }
        else if (errorStatement is ElseClauseSyntax)
        {
            var newElseStatement = errorStatement as ElseClauseSyntax;
            newElseStatement = newElseStatement.WithStatement(SyntaxFactory.Block(newElseStatement.Statement)).WithAdditionalAnnotations(Formatter.Annotation);
            toReplace = newElseStatement;
        }
    
        if (toReplace != null)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(errorStatement, toReplace);
            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;
        }
        else
        {
            return null;
        }
    }

   Her oppretter vi nye if- eller else-statements med curly brackets og legger det p� dokumentet.

11. Start l�sningen igjen og opprett en ny Console Application. Skriv en if-else-statement uten brackets og klikk p� lysp�ren som dukker opp ved siden av warningen. Du skal n� kunne velge "Add brackets".