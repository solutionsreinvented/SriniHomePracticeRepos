import { eventBus } from '../core/EventBus.js';

export class SelectTool {
  activate()   { document.body.style.cursor = 'default'; }
  deactivate() { document.body.style.cursor = 'default'; }
  // Clicks handled by SelectionManager when no active tool override
  onClick()    {}
  onMouseMove(){}
}
