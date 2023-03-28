import { Home } from "./components/Home";
import NewTicket from "./components/NewTicket";
import Ticket from "./components/Ticket";
import Tickets from "./components/Tickets";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/tickets',
        element: <Tickets />
    },
    {
        path: '/tickets/new',
        element: <NewTicket />
    },
    {
        path: '/tickets/:ticketId',
        element: <Ticket />
    }
];

export default AppRoutes;
