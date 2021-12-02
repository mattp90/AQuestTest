# AQuestTest
Technical test for AQuest

## Tecnologie utilizzate
- MVC 4
- .Net Framework 4.8
- EntityFramework 6
- JQuery
- Microsoft SQL Server

## Descrizione
L'applicativo si presenta con una schermata iniziale nella quale vegnono riportati tutti gli ordini salvati sul database; il dettaglio dell'ordine è consultabile solamente se l'ordine è in stato "In elaborazione" o "Confermato". 
E' infatti stata introdotta una logica di stati per poter gestire meglio la pagina di "Riepilogo ordine".
Gli **Stati** degli ordini sono salvati nella tabella **OrdersStates** e sono i seguenti:
- In elaborazione
- Confermato
- Cancellato

Nella schermata del Dettaglio Ordine è possibile andare a modificare la quantità di articoli per l'ordine selezionato; tutte le operazioni di aggiunta/rimozione articoli viene effettuata tramite chiamate Ajax. 

In questa schermata sarà poi possibile andare ad applicare un codice sconto all'ordine; i vari coupons si trovano nella tabella **Coupons** e la quantità di sconto è identificata dalla colonna **"PercentageDiscount"**; in questa colonna viene definita la quantità di percentuale di sconto che, una volta selezionata per l'ordine, andrà a scontare il prezzo totale dell'ordine.

Procedendo con le operazioni, si arriva alla schermata delle informazioni per l'utente. Questa gestione è, passatemi il termine, abbastanza sbrigativa. Non vi è infatti una gestione dell'utente registrato a database e non vi è alcuna possibilità di effettuare il login nel sistema; ora, per ogni ordine effettuato e confermato, le informazioni dell'utente vengono salvate sul database. Questo comporta una possibile ridondaza delle informazioni.

Una volta confermati i dati dell'utente si arriva all'ultima pagina di Checkout; in questa schermata viene proposto un riepilogo generale sia dell'ordine sia delle informazioni dell'utente. Se, infine, si procede con il pagamento si verrà allora indirizzati in ambiente PayPal Sandbox; una volta effettuato il pagamento avverrà un reindirizzamento alla pagina di Home dell'applicativo.

## Info utili
- Una volta effettuato il restore del database (possibile tramite file .bak o tramite script di insert presenti nella cartella Database) assicurarsi che il nome del database sia lo stesso impostato a **Web.Config** nella chiave di configurazione **DatabaseName**.
- Microsoft SQL Server è stato configurato da permettere l'accesso tramite Autenticazione Windows.
- Utente di login per la Sandbox PayPal
    - Username: sb-wc1uj8799842@personal.example.com
    - Password: !/9azaTN

## TO DO
### In questa sezione vengono riportati quali potrebbero essere aspetti migliorativi generali dell'applicazione. Alcuni di questi punti apparentemente non troppo invasivi non sono stati implementati per motivazioni di tempo
- Possibilità di aggiungere nuovi prodotti all'ordine
- Pagina di Login
- Gestione di registrazioni utenti
- Gestione dei Log (NLog.dll)
- Gestione ottimizzata delle eccezioni
