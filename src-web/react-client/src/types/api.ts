type ApiErrorResponse = {
  title: string;
  type: string;
  status: number;
  detail: string;
};

export type BadRequestResponse = ApiErrorResponse & {
  errors?: { code: string; message: string }[];
};

export type ConflictResponse = ApiErrorResponse;

export type UnauthorizedResponse = ApiErrorResponse;
