import { HttpHeaders } from "@angular/common/http";

export const getHeaders = (token: any) => { return { headers: new HttpHeaders({ Authorization: `Bearer ${token}` }) }}
