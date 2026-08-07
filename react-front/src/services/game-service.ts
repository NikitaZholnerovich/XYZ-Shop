import type { PaginatedResponse } from "../types/pagination";
import type { Game } from "../types/game";

interface GetGamesParams {
  page?: number;
  pageSize?: number;
  maxPrice?: string;
  genreId?: string;
  sortBy?: string;
  sortDirection?: string;
}

export async function getGames(
  params: GetGamesParams = {},
): Promise<PaginatedResponse<Game>> {
  const queryParams = new URLSearchParams();
  if (params.page) queryParams.append("page", params.page.toString());
  if (params.pageSize)
    queryParams.append("pageSize", params.pageSize.toString());
  if (params.genreId) queryParams.append("genreId", params.genreId.toString());
  if (params.maxPrice)
    queryParams.append("maxPrice", params.maxPrice.toString());
  if (params.sortBy) queryParams.append("sortBy", params.sortBy);
  if (params.sortDirection)
    queryParams.append("sortDirection", params.sortDirection);

  const query = queryParams.toString();
  const response = await fetch(
    `/api/Catalog/GetGames${query ? `?${query}` : ""}`,
  );

  if (!response.ok) {
    throw new Error(`Request error: ${response.status}`);
  }

  return response.json();
}

export async function getGame(id: number): Promise<Game | null> {
  const response = await fetch(`/api/Catalog/GetGameDetails?id=${id}`);

  if (response.status === 404) {
    return null;
  }

  if (!response.ok) {
    throw new Error(`Request error: ${response.status}`);
  }

  return response.json() as Promise<Game>;
}
