export interface Customer {
  id: number;        // .NET int -> TS number
  name: string;      // .NET string -> TS string
  email: string;     // .NET string -> TS string
  balance: number;   // .NET decimal/double -> TS number
  createdAt: string; // .NET DateTime -> TS string (ISO Format)
}
