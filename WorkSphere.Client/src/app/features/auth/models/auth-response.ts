import {User} from './user';

export class AuthResponse {
  public user: User;
  public token: string;
  public expiresIn: number;
}
