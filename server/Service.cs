using Microsoft.EntityFrameworkCore;
using TodoApi;

class Service
{
    readonly ToDoDbContext toDoDbContext;
    public Service(ToDoDbContext toDoDbContext)
    {
        this.toDoDbContext = toDoDbContext;
    }
    public async Task<IEnumerable<Item>> GetItem()
    {
        try
        {
            return await toDoDbContext.Items.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task<Item> AddItem(Item item)
    {
        try
        {
            var addItem = await toDoDbContext.Items.AddAsync(item);

            await toDoDbContext.SaveChangesAsync();

            return addItem.Entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }


    public async Task<Item> UpdateItem(int id, Item updatedItem)
    {
        try
        {
            var existingItem = await toDoDbContext.Items.FindAsync(id);

            if (existingItem == null)
            {
                return null;
            }
            if(updatedItem.Name!=null)
                existingItem.Name = updatedItem.Name;
            if(updatedItem.IsComplete!=null)
                existingItem.IsComplete = updatedItem.IsComplete;

            await toDoDbContext.SaveChangesAsync();

            return existingItem;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }


    public async Task<Item> DeleteItem(int id)
    {
        try
        {
            var existingItem = await toDoDbContext.Items.FindAsync(id);

            if (existingItem == null)
            {
                return null;
            }

            toDoDbContext.Items.Remove(existingItem);

            await toDoDbContext.SaveChangesAsync();

            return existingItem;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            return null;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Invalid operation: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return null;
        }
    }


}
