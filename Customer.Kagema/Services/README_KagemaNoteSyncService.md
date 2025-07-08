# Kagema Note Sync Service Override

## Purpose
This service reduces mobile data synchronization load by filtering notes (and their attached files) to only include those relevant to the technician's assigned service orders.

## Implementation Details

### What it does:
1. **Filters by Technician Assignment**: Only syncs notes related to service orders where the technician is assigned via dispatch
2. **Excludes Closed Orders**: Only syncs notes from service orders that are not in closed/completed status
3. **Maintains General Notes**: Still syncs notes that don't have a reference key (general notes)

### Status Keys Considered "Open":
- NewKey
- ReadyForSchedulingKey
- ScheduledKey
- PartiallyReleasedKey
- ReleasedKey
- InProgressKey
- PartiallyCompletedKey
- PostProcessingKey
- ReadyForInvoiceKey

### Status Keys Excluded (Closed):
- CompletedKey
- InvoicedKey
- Other closed statuses

## Configuration
The service is automatically registered in `KagemaModule.cs` and replaces the default `NoteSyncService`.

## Expected Impact
- **Reduced Data Transfer**: Technicians only sync files from their assigned, active service orders
- **Faster Sync Times**: Less data to download and process
- **Better Mobile Performance**: Reduced storage usage on mobile devices
- **Maintained Functionality**: All core features remain unchanged

## Testing
To verify the implementation is working:
1. Check that technicians only see notes/files from their assigned service orders
2. Verify that notes from closed service orders are not synchronized
3. Confirm that general notes (without reference keys) are still available