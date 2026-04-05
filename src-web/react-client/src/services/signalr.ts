import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

let connection: HubConnection | null = null;

export const createConnection = (url: string) => {
  connection = new HubConnectionBuilder()
    .withUrl(url)
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

  return connection;
};

export const getConnection = () => connection;
